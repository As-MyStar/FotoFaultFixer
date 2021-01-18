# FotoFaultFixer
A .net5 application to fix faults in old photos and images.

See an example of it in-action here: https://www.youtube.com/watch?v=9h9NCo1y1H4

## Screenshots
![Screenshot of version Alpha1](https://github.com/FiddlyDigital/FotoFaultFixer/blob/CommandExecution/FotoFaultFixerUI/screenshots/Alpha3.PNG?raw=true)

## Explanation
The application is based around the concept of easily fixing the faults in photos and images.
Unlike photoshop or lightroom; it's extremely simple to use and has kept non-tech users in mind.

This tool originated as an implementation of some of the algorithms from the book "Modern Algorithms for Image Processing" by Vladimir Kovalevsky.
What started as a small console-based POC has slowly spun out into a full-blown multi-layer Application.

The UI is a WPF App (with MVVM archicecture) which calls into an independant .Net5 Class Library to handle image interactions.
All Image actions have been custom-coded for speed and portability.

There's still plenty of work to be done, but the current state is well-architected, easy to read, and has plenty of scope for extension/improvement.
If you're interested in contibuting please file a PR or DM me via the usual channels (Twitter, Insta, etc...)
