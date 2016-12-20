./make.sh
echo "Creating archive ..."
rm -rf boxtal.zip
zip -r boxtal.zip src doc/index.html doc/js doc/css doc/logo.png doc/assembly bin > /dev/null
