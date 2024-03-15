all: pldotnet-whitepaper-mar24.pdf

# Local software dependencies:
#     - pdflatex and bibtex, from xetex (or wherever)
#     - python, for pygments, which latex(minted) needs

SMALL_IMGS=img/pldotnet-comparison-pljava.png img/pldotnet-comparison-pllua.png img/pldotnet-comparison-plperl.png img/pldotnet-comparison-plpgsql.png img/pldotnet-comparison-plpython.png img/pldotnet-comparison-plr.png img/pldotnet-comparison-pltcl.png img/pldotnet-comparison-plv8.png 
BIG_IMGS=img/pldotnet-performance-comparison.png img/pldotnet-sequence-diagram.png

img/%.png:
	cd img && basename $@ | xargs make

.venv.created:
	make venvclean # sleazy dep
	mkdir venv
	(cd venv && python3 -m virtualenv .)
	. venv/bin/activate && pip3 install pygments
	touch .venv.created

interior_pdf: .venv.created $(BIG_IMGS)
	. venv/bin/activate && \
	pdflatex -shell-escape pldotnet-whitepaper-mar24.tex # `-shell-escape` is for minted

pldotnet-whitepaper-mar24.pdf: pldotnet-whitepaper-mar24.tex
	(cd img && make)
	make interior_pdf
	bibtex pldotnet-whitepaper-mar24
	rm -f $@
	make interior_pdf

venvclean:
	rm -rf .venv.created venv/

lightclean:
	rm -rf pldotnet-whitepaper-mar24.{aux,out,toc,log,lot,lof} _minted-pldotnet-whitepaper-mar24/

clean: lightclean
	rm -f pldotnet-whitepaper-mar24.pdf
	cd img && make clean

veryclean: venvclean clean
	cd img && make veryclean
