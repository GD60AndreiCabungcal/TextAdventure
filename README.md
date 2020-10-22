- Name: Andrei Cabungcal
- VFS Username: gd60andrei
- Date: 10/7/2020
- Assignment Title: Text Adventure
- Application Title: Text Adventure

- Link to Clone: https://github.com/GD60AndreiCabungcal/TextAdventure.git (main branch)

Programming Requirements:
- Illustrate the main game loop (Program.cs, lines 41-50)
- Create a 'class' and declare 'objects' based on that class (Player.cs; Program.cs, line 34)
- Create a class with a 'constructor' and use it to initialize an object (Weapon.cs, lines 14-18; Simulator.cs, line 34)
- Create a class which uses inheritance to derive behavior from another class (Enemy.cs; Player.cs)
- Illustrate at least one overloaded method (DecisionHandler.cs, lines 11-40, lines 42-48)
- Illustrate error checking on parameters, initialized properties, and validation on all user input (DecisonHandler.cs, lines 22-35)
- Illustrate best practices in coding, one class per file, short methods, descriptive variable names, Goldilocks level of comments, and 
  the use of know language features to complete the assignment

How To Use Program:
    Two types of player input:

    Decisions:
        - formatted as:
        Ex. What do you want to do?
        A) option a
        B) obtion b
        C) option c
        - answer them using the left letter(not case sensitive)
        Ex. 
        a
        - decisions won't work if the input is incorrect

    Commands:
        - formatted as:
        Ex. What do you want to do?
        command1 arg1 <- manditory literal argument
        command2 <arg2> <- optional literal argument
        command3 $arg3 <- '$', object name argument
        command4 arg4a|arg4b <- '|', choice argument
        - answer them using the argument rules above
        Ex. 
        command1 arg1
        command2 OR command2 arg2
        command3 nameOfObject
        command4 arg4a OR command4 arg4b
        - not case sensitive
        - argument rules may be used in conjunction to eachother

        


