PUML_JAR=/opt/homebrew/Cellar/plantuml/1.2022.14/libexec/plantuml.jar

LANGUAGES := plpgsql plfsharp plperl plpython pltcl pllua pljava plv8 plr

IMAGE_TARGETS := $(addprefix pldotnet-comparison-, $(addsuffix .png, $(LANGUAGES)))

NEWEST_CSV=$(shell ls pldotnet-performance-data-*.csv | sort | tail -n 1)

# Local software dependencies:
#     - ImageMagick, for `convert` and `montage`
#     - plantuml
#     - python with virtualenv (it's standard)

all: pldotnet-performance-comparison.png pldotnet-sequence-diagram.png heatmap.png

clean:
	rm -rf *png

venvclean:
	rm -rf .venv.created venv/

veryclean: clean venvclean

.venv.created:
	make venvclean
	mkdir venv
	(cd venv && python3 -m virtualenv .)
	. venv/bin/activate && \
	pip3 install kaleido numpy pandas plotly seaborn
	touch .venv.created

pldotnet-performance-comparison.png: $(IMAGE_TARGETS)
	montage $^ -tile 3x3 -geometry +2+2 $@

$(IMAGE_TARGETS): pldotnet-comparison-%.png: pldotnet-performance-graph-maker.py $(NEWEST_CSV) .venv.created
	. venv/bin/activate && \
	./pldotnet-performance-graph-maker.py $(NEWEST_CSV) plcsharp $*

heatmap.png: heatmap-big.png
	# The heatmap is too big, so we reduce it
	convert $< -resize 15% -filter Lanczos $@

heatmap-big.png: .venv.created pldotnet-performance-heatmap-maker.py $(NEWEST_CSV)
	. venv/bin/activate && \
	python pldotnet-performance-heatmap-maker.py $(NEWEST_CSV) $@

pldotnet-sequence-diagram.png: ./pldotnet-sequence-diagram.puml
	plantuml -Tpng ./pldotnet-sequence-diagram.puml

benchmark-tests:
	cd .. && cd .. && make benchmark-tests NRUNS=$(NRUNS) && cd docs/img
