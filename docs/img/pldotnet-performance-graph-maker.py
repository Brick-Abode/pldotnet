#!/usr/bin/env python3

# To install dependencies:
# pip3 install kaleido numpy pandas plotly

# To create graphs:
# for lang in plpython plpgsql pljava plperl pllua pltcl plr plv8; do ./processor.py pldotnet-performance-data.csv plcsharp $lang; done

import os, sys, csv
import plotly.graph_objects as go
import plotly.express as px
import numpy as np
import pandas as pd

########################################
def find_slot(val):
    # val is a number between 0.0 and 0.5
    # returns the relative weighting of 0.5
    return val / 0.5

def rgbify(r, g, b):
    # You can use the '{:02x}'.format(n) string formatting code.
    vals = [ '{:02x}'.format(int(255*n)) for n in (r, g, b) ]
    return "".join(vals)

def color(slot, number):
    # r is 0.0, g is 0.5, b is 1.0
    # 0.00 = (1, 0, 0)
    # 0.25 = (.5,.5,0)
    assert(slot <= number)
    assert(slot >= 0.0)
    val = slot / number
    if val == 0.5:
        r, g, b = (0.0, 1.0, 0.0)
    elif val < 0.5:
        g = find_slot(val)
        r = 1.0 - g
        b = 0.0
    elif val > 0.5:
        nval = val - 0.5
        b = find_slot(nval)
        g = 1.0 - b
        r = 0.0
    else:
        raise Exception("Illegal slot/number: %s/%s" % (slot, number))
    return rgbify(r, g, b)


# if __name__ == "__main__":
#     import sys
#     # print("d1")
#     (slot, number) = (int(sys.argv[1]), int(sys.argv[2]))
#     # print("d2")
#     rgb = color(slot, number)
#     print("slot %s, number %s, rgb %s" % (slot, number, rgb))
#     # print("d3")

########################################
header = "#" * 40

def make_perc(val, inverted=True):
    if inverted:
        val = 1.0 / val
    return "%.2f" % ((val-1.0) * 100.0)

def debug(msg, enabled=True):
    if enabled:
        print("# DEBUG: %s" % msg)

# def ydump(data):
#     print(header)
#     print(yaml.dump(data))
#     print(header)

def average(vals):
    average = (sum(vals) / float(len(vals)))
    debug("Returning average %s" % average, False)
    return average

########################################

def make_av(data, l1=0, l2=1):
    sum1 = sum(data[l1][1])
    sum2 = sum(data[l2][1])
    return sum1/sum2

def make_fig(filename, data):
    av = make_av(data) # the average score
    names = [name for (name, datum) in data]
    title = "%s takes %s%% longer than %s" % (names[0], make_perc(av, False), names[1])
    if av < 1:
        title = "%s takes %s%% longer than %s" % (names[1], make_perc(av), names[0])
    # title += "<br>(pl/dotnet is 1.0; less is better)"
    print("# DEBUG: title is '%s'" % title)
    data2 = dict(data)

    dim = int(os.environ.get("IMGSZ", "512")) # pixel size

    fig = px.line(pd.DataFrame(data2), y=names, log_y=False)

    fig.add_shape(
        type='line',
        x0=0,
        y0=1.0/av,
        x1=len(data[0][1]),
        y1=1.0/av,
        # text=("Average: %s" % (1.0/av)),
        line={
              "color": "Green",
              "dash": "dash",
              }
    )

    # a quick hack to make the plpgsql label more visible
    # better would be to also reduce the arrow, but it will do
    yshift=0
    # our performance is now so close that we don't need this!
    # if "plpgsql" in filename:
        # yshift=int(dim * -0.01)

    fig.add_annotation(
                x=(3 * len(data[0][1])/4),
                y=(1.0/av),
                yshift=yshift,
                text=("Average: %.2f%%" %  (100.0/av)),
                # color="Green"
            )

#     fig.data[0].update(
#         annotations=[
#             dict(
#                 x=len(data[0][1])/2,
#                 y=1.0/av,
#                 xref="x",
#                 yref="y",
#                 text=("Average: %.2f" %  (1.0/av)),
#                 showarrow=False,
#                 ax=0,
#                 ay=0
#             )
#         ]
#     )

    fig.update_layout(
        title=title,
        xaxis_title="",
        yaxis_title="Relative execution time",
        legend_title="Language",
        title_x=0.5,
        font=dict(
            family="Open Sans",
            size=12,
            color="Black"
        )
    )
    fig.update_xaxes(showticklabels=False)
    fig.update_layout(width=dim, height=dim)

    fig.write_image(filename)

def read_data(filename):
    # returns dict[language][test] = execution_time
    with open(filename, newline='') as csvfile:
        reader = csv.reader(csvfile, delimiter='\t')
        first = True
        ret = {}
        for row in reader:
            if first:
                header = row
                for pl in row[1:]:
                    ret[pl] = {}
                first = False
            else:
                for (pl, datum) in zip(header[1:], row[1:]):
                    ret[pl][row[0]] = datum
        return ret

def compare(data, lang1, lang2):
    # returns dict[test] = score
    comparison = {}
    total_count = 0
    reject_count = 0
    missing_count = 0
    outlier_count = 0
    tests = sorted(data[lang1].keys())
    debug("Starting comparison with %s tests" % len(tests), False)
    for test in tests:
        total_count = total_count + 1
        debug("Comparing test %s between languages %s and %s" % \
                (test, lang1, lang2), False)
        if test in data[lang2]:
            if test in comparison:
                raise Exception("Duplicate test: %s" % test)
            if data[lang2][test] not in ('-', '', 'NULL', 'null'):
                score1 = float(data[lang1][test])
                score2 = float(data[lang2][test])
                c = score1/score2
                debug("c(%s) = score1(%s)/score2(%s)" %
                      (c, score1, score2), False)
                if c < 10.0 and c > 0.1:
                    comparison[test] = c
                else:
                    debug("Skipping outlier test %s: %s" % (test, c))
                    outlier_count = outlier_count + 1
            else:
                debug("Rejecting null test: %s=%s" % (test, data[lang2][test]))
                reject_count = reject_count + 1
        else:
            debug("Test %s is missing for language %s" % (test, lang2))
            missing_count = missing_count + 1
    debug("comparing %s to %s, got %s data points, rejected %s, missing %s, outlier %s, returning %s" % \
            (lang1, lang2, total_count, reject_count, missing_count, outlier_count, len(comparison)), False)
    return comparison

def graph_compare(data, l1, l2, inverted=True):
    # creates a graph of the comparison
    # l1 is always 1.0
    comparison = compare(data, l1, l2)
    av = average(comparison.values())
    debug("Average 2 is %s" % av, False)
    vals1 = sorted(comparison.values())
    vals2 = [1.0 for x in vals1]
    # This is dumb, but I'm too lazy to fix it properly
    if inverted:
        vals2 = [1.0 / x for x in vals1]
        vals1 = [1.0 for x in vals2]
    data = [
            (l1, vals1),
            (l2, vals2),
            ]
    filename="pldotnet-comparison-%s.png" % l2
    av = make_av(data)
    debug("Language %08s takes on average %06s%% as long as %s." %
          (l2, "%.2f" % (100.0 / av), l1))
    make_fig(filename, data)

def graph_compare_n(data, *langs, inverted=True):
    # creates a graph of the comparison
    # langs[0] is always 1.0
    benchmark_lang = langs[0]

    # we use the comparison to give us the sort order on the tests
    comparison = compare(data, langs[0], langs[1])
    test_ordered = [x for (x,y) in sorted(comparison.items(), key=lambda pair: pair[1])]
    debug("Before filtering, have %s tests" % len(test_ordered), False)
    av = average(comparison.values())
    debug("Average 2 is %s" % av, False)

    # default_exclude_categories = "Recursive"
    default_exclude_categories = ""
    exclude_categories = os.getenv("EXCLUDE_CATEGORIES", default_exclude_categories).split(",")

    # make_fig needs this:
    # xdata = [ (l1, vals1), (l2, vals2),... ]
    xdata = [ [] for lang in langs ]
    rejects = 0
    for test in test_ordered:

        valid = True

        # we skip excluded categories
        if data["Category"][test] in exclude_categories:
            debug("Skipping data['Category'][%s]=%s" % (test, data["Category"][test]))
            valid = False
        else:
            # debug("Including data['Category'][%s]=%s" % (test, data["Category"][test]))
            # we skip invalid values
            for lang in langs:
                if data[lang][test] in ('-', '', 'NULL', 'null'):
                    debug("Skipping data[%s][%s] on null" % (lang, test))
                    valid = False
                    break

        # if data is valid, then we add it
        if valid: 
            benchmark_val = float(data[benchmark_lang][test])
            for n in range(len(langs)):
                lang = langs[n]
                xdata[n] += [float(data[lang][test]) / benchmark_val]
        else:
            rejects = rejects + 1

    debug("After filtering, comparing %s to %s, got %s data points, rejected %s" % \
            (langs[0], langs[1], len(xdata[0]), rejects))
    debug("Using tests %s" % sorted( data[langs[0]].keys() ), False)

    ydata = [(langs[n], xdata[n]) for n in range(len(langs))]
    filename="pldotnet-comparison-%s.png" % langs[1]
    av = make_av(ydata)
    print("Language %08s takes on average %06s%% as long as %s." %
          (langs[1], "%.2f" % (100.0 / av), langs[0]))
    make_fig(filename, ydata)

def report(comparison):
    for(test, score) in comparison.values():
        pass

########################################
def main(argv):
    filename = argv[1]
    data = read_data(filename)
    if len(argv) == 4:
        # comparison = compare(data, argv[2], argv[3])
        # graph_compare(data, argv[2], argv[3])
        graph_compare_n(data, *argv[2:])
    elif len(argv) > 4:
        # comparison = compare(data, *argv[2:])
        graph_compare_n(data, *argv[2:])
    else:
        raise Exception("Usage: %s <csv> <lang1> <lang2> (<langN...>*)" % argv[0])

main(sys.argv)

