echo "Cleaning ..."
rm -rf  doc/assembly/* bin/*
mkdir -p tmp
mkdir -p tmp/doc
mkdir -p bin
echo "Compilation ..."
dmcs /target:library /optimize -out:bin/boxtal.dll src/*.cs /doc:doc/doc.xml -r:System.Web.dll  > /dev/null
echo "Generating documentations ..."
monodocer -pretty -importslashdoc:doc/doc.xml -assembly:bin/boxtal.dll -path:tmp  > /dev/null
mdoc export-html --template doc/style.xsl -o doc/assembly tmp  > /dev/null
rm -rf tmp
rm doc/doc.xml
