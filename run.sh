rm *.dll
rm *.exe

ls -l

mcs -target:library -r:System.Drawing.dll -r:System.Windows.Forms.dll -out:MoveBall.dll MoveBall.cs

mcs -r:System -r:System.Windows.Forms -r:MoveBall.dll -out:ball.exe MoveBallMain.cs

ls -l

./ball.exe