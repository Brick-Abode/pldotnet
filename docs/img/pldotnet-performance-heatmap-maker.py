import pandas as pd
import seaborn as sns
import matplotlib.pyplot as plt
import numpy as np
from itertools import groupby
import sys, os, functools


def manipulate_data_frame(filtered_df, fact, pl_reference):
    # Manipulate DataFrame to generate the heatmap
    heatmap_df = filtered_df.drop(["Category", "Test Case"], axis=1)
    heatmap_df = heatmap_df.div(heatmap_df[pl_reference], axis=0)

    # Calculate the mean, max, and min of each column
    mean_values = heatmap_df.mean()
    max_values = heatmap_df.max()
    min_values = heatmap_df.min()

    new_rows = pd.DataFrame(
        [min_values, max_values, mean_values], columns=heatmap_df.columns
    )
    heatmap_df = pd.concat([heatmap_df, new_rows], ignore_index=True)

    # sort the DataFrame based on the mean values of the columns
    heatmap_df = heatmap_df.reindex(mean_values.sort_values().index, axis=1)

    # Compute natural logarithmic
    heatmap_df = np.log(heatmap_df)
    heatmap_df = heatmap_df.applymap(lambda x: x * abs(fact) if (x * fact < 0) else x)

    mean_values = np.concatenate((["Average", "Results"], mean_values))
    min_values = np.concatenate((["Best performance", "Results"], min_values))
    max_values = np.concatenate((["Worst performance", "Results"], max_values))
    new_rows = pd.DataFrame(
        [min_values, max_values, mean_values], columns=filtered_df.columns
    )
    filtered_df = pd.concat([filtered_df, new_rows], ignore_index=True)

    return heatmap_df, filtered_df


# function from https://stackoverflow.com/questions/58854335/how-to-label-y-ticklabels-as-group-category-in-seaborn-clustermap
def add_line(ax, xpos, ypos):
    line = plt.Line2D(
        [ypos, ypos + 0.25], [xpos, xpos], color="black", transform=ax.transAxes
    )
    line.set_clip_on(False)
    ax.add_line(line)


# function from https://stackoverflow.com/questions/58854335/how-to-label-y-ticklabels-as-group-category-in-seaborn-clustermap
def label_len(my_index, level):
    labels = my_index.get_level_values(level)
    return [(k, sum(1 for i in g)) for k, g in groupby(labels)]


# function from https://stackoverflow.com/questions/58854335/how-to-label-y-ticklabels-as-group-category-in-seaborn-clustermap
def label_group_bar_table(ax, df):
    xpos = -0.25
    scale = 1.0 / df.index.size
    # print(f"scale: {scale}")
    for level in range(df.index.nlevels):
        # print(f"level: {level}")
        pos = df.index.size
        # print(f"pos: {pos}")
        for label, rpos in label_len(df.index, level):
            # print(f"rpos: {rpos}")
            add_line(ax, pos * scale, xpos)
            pos -= rpos
            aux = 0.25 if level == 0 else 0.5
            lypos = (pos + aux * rpos) * scale
            ax.text(
                (xpos + 0.125 if level == 0 else xpos + 0.0725),
                lypos,
                label,
                ha="center",
                transform=ax.transAxes,
                fontsize=16 if level == 0 else 20,
            )
        add_line(ax, pos * scale, xpos)
        xpos -= 0.15


def create_heatmap(pngfile, filtered_df, pl_reference, category=None):
    # Constant defined to increase the scale of the value
    # If positive, scale the green values (better than the reference language)
    # If negative, scale the red values (worse than the reference language)
    fact = 5.0

    # Manipulated DataFrame with log values
    heatmap_df, filtered_df = manipulate_data_frame(filtered_df, fact, pl_reference)

    # Create figure
    a = 0.5
    fig = plt.figure(figsize=(20, a * len(heatmap_df.index)))

    # Create the heatmap
    ax = sns.heatmap(
        heatmap_df,
        cmap="RdYlGn_r",
        annot=True,
        fmt=".4f",
        linewidth=0.5,
        center=0.0,
        annot_kws={"size": 16, "ha": "center", "va": "center"},
        cbar=False,
    )

    # Set information on the heatmap
    ax.set_yticklabels(filtered_df["Test Case"], rotation=0, fontsize=16)
    ax.set_xticklabels(heatmap_df.columns, fontsize=16)
    ax.set_xlabel("Procedural languages", fontsize=20)
    ax.set_ylabel("Test name", fontsize=20)

    # Set the percentage values to the heatmap
    for text in ax.texts:
        if text.get_text() != "0.0000":
            log_value = float(text.get_text())
            log_value = log_value / abs(fact) if log_value * fact < 0.0 else log_value
            original_value = np.exp(log_value)
            percentage_value = (original_value - 1.0) * 100.0
            text.set_text("{:.1f}%".format(percentage_value))
        else:
            text.set_text("")

    if category is None or category == "":
        plt.title(
            f"Performance results against {pl_reference} (%)",
            fontsize=24,
        )
        # Below 3 lines remove default labels
        labels = ["" for item in ax.get_yticklabels()]
        ax.set_yticklabels(labels)
        ax.set_ylabel("")
        filtered_df = filtered_df.set_index(["Test Case", "Category"])
        label_group_bar_table(ax, filtered_df)
        fig.subplots_adjust(bottom=0.1 * filtered_df.index.nlevels)
        plt.savefig(pngfile, dpi=300, bbox_inches="tight")
    else:
        plt.title(
            f'Performance results for "{category}" tests against {pl_reference} (%)',
            fontsize=24,
        )
        plt.savefig(f"heatmap_{category}.png", dpi=300, bbox_inches="tight")

    # plt.show()


def filter_data_by_category(data, category):
    filtered_df = data.loc[data["Category"] == category]
    return filtered_df


def main(argv):
    # Read in the CSV file
    if len(argv) != 3:
        raise Exception("Usage: %s <csv-input> <png-output>" % argv[0])
    csvfile = argv[1]
    pngfile = argv[2]
    df = pd.read_csv(csvfile, delimiter="\t")

    # USAGE: you can pass "EXCLUDE_CATEGORIES=Array,Bool" as an environmental variable
    # USAGE: to see all categories, use "EXCLUDE_CATEGORIES="
    exclude_categories = os.getenv("EXCLUDE_CATEGORIES", "").split(",")
    exclude_tests = [ (df["Category"] != cat) for cat in exclude_categories ]
    exclude_test = functools.reduce(lambda x, y: x & y, exclude_tests)

    # USAGE: you can pass "PL_REFERENCE=plpython" as an environmental variable
    pl_reference = os.getenv("PL_REFERENCE", "").lower() or "plpython"

    # Whether should create the heatmap for each category
    by_category = False

    if by_category:
        # Create a list of unique categories
        categories = df["Category"].unique()
        for category in categories:
            filtered_df = filter_data_by_category(df, category)
            create_heatmap(pngfile, filtered_df, pl_reference, category)
    else:
        # print("# DEBUG: pre-filtering, df[%s] is:\n%s" % (len(df), df))
        df = df.loc[exclude_test]
        # print("# DEBUG: post-filtering, df[%s] is:\n%s" % (len(df), df))
        # print("# DEBUG: tests are %s" % sorted(df["Test Case"]))
        create_heatmap(pngfile, df, pl_reference)


main(sys.argv)
