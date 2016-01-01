del _*.*
..\bin\ps2pdf.exe -rotate 180 -subject "subject" -producer "producer" -creator "creator" -ownerpwd owner -keylen 2 -encryption 3900 test.eps _test.eps.pdf
..\bin\ps2pdf.exe -rotate 90 -subject "subject" -producer "producer" -creator "creator" -ownerpwd owner -keylen 2 -encryption 3900 test.ps _test.ps.pdf
..\bin\ps2pdf.exe -mergepdf "_test.eps.pdf|_test.ps.pdf" _merged.pdf
..\bin\ps2pdf.exe -burstpdf _merged.pdf _merged.pdf-burst
..\bin\ps2pdf.exe -burstpdf test.pdf _test.pdf-burst
..\bin\ps2pdf.exe -mergepdf test.pdf _test-merged.pdf
..\bin\ps2pdf.exe -pdf2ps _merged.pdf _merged.pdf.ps
..\bin\ps2pdf.exe -pdf2ps test.pdf _test.pdf.ps

pause
