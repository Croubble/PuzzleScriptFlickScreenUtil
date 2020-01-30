## PUZZLESCRIPT Command Prompt Flick Utility !! ALPHA !!

### Intro

Do you want to make giant flickscreen puzzlescript game without manually copy and pasting individual level into and out of the giant flickscreen level? If so, you've come to the right utility! 

### Prerequisites
Windows

### How to use
1. Download and extract the zip / clone the repo.
2. Run the PuzzlescriptFlickUtil.exe to see how the input file is turned into the output file.
3. Try deleting the input file, then renaming output.txt to input.txt, and running the .exe again. 
4. Copy and paste your own puzzlescript game to input, annotate the levels similarily to the example, and go to town!

### Example 

Our inputted flickscreen levels. Oh its pretty! We save it as input.txt
```
(level w1 h1)
#.#
#a.
###

(level w2 h1)
#.#
.b#
###

(level w2 h2)
###
.c#
#.#

(level w1 h2)
###
#d.
#.#
```

We run Croubbles ~~buggy~~ excellent PuzzleScriptFlickUtil.exe. We open the newly created output.txt to unveil a brand new file.

```
(flickscreen 3x3)
######
#d..c#
#.##.#
#.##.#
#a..b#
######
```

if we were to then rename output to input, and run the .exe again ,we'd get the old input file back in output. Nice!


### Running the tests

This code doesn't have any tests yet because I'm just, like, so good at code that this project has no bugs. 

I lie. This project has bugs. If you've ever met bugs bunny, then just mentally remove the bunny from the equation, and that's the bugs were talking about. Woah. I'll test it up later, this thing is alpha as right now.


#### TODO

- Cleanup and generally improve this readme [ ]
- The big one: Proper error messaging. When the parser fails due to invalid input, it should report why it failed, and the line number the failure occured on if relevant [ ] 
- Editing and refactoring the code to not have hilariously bad names. Also to not be as complicated as a cosmic horror antagonist [ ] 
- Option if input.txt isn't detected to manually input the file location we should read from. [ ] 
- if the option file already exists, option to name the file so we don't accidentally overwrite a useful file. [ ] 

The MIT License (MIT)

Copyright (c) 2020 Croubble

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
