# ProjectEuler
Multithreaded WinForms framework for solving [Project Euler](https://projecteuler.net/) problems.

## Adding a new problem
1. Copy and paste Problem000.cs in the Problems folder.
1. Rename the file Problemxxx, where xxx is the number of Project Euler problem you are working.
1. Rename the class within to Problemxxx as well.  The Problems framework will use this to build a list of singletons for each problem.
1. 
1.
1.

## Some additional features
* Any abstract class inheriting from Problem will not be instantiated in the list.  This allows you to reuse code in similar problems.  e.g. 81/82/83
* 