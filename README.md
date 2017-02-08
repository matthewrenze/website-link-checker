# Website Link Checker
*Purpose:* Checks a website for broken links

*Usage:* websitelinkchecker.exe [url] [-f]

* url - the URL for the website to be checked
* -f  - enables full logging

*Example:* websitelinkchecker.exe http://www.matthewrenze.com -f

*Output:* 
+ First, the page that is being checked is output to the console window.
* Next each link on the page is output indicting whether it passed or failed.
* Then the next page on the website is listed and so on.

If full logging is enabled, then both all links will be displayed.  
If full logging is disabled, then only broken links will be displayed.

*Sample Output:*  
http://www.matthewrenze.com/  
PASS: http://www.matthewrenze.com/page-1  
FAIL: http://www.brokensite.com/ (ProtocolError - Forbidden)  
http://www.matthewrenze.com/page-1  
PASS: http://www.workingsite.com  
...

*Tip:*
If you would like to write the output directly to a file rather than to the console, just pipe the output into a new file. 
For example, the command "websitelinkchecker.exe http://www.matthewrenze.com/ > Output.txt" would pipe the console output into a file called "Output.txt".
You will not, however, see any progress occur in the console window until the application has completed.
