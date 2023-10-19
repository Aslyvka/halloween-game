# halloween-game
This project is a 3 level Halloween themed game developed in c# UWP.
User Manual
This project is a Halloween-themed game created using C#. It has 3 Levels of difficulty, timer, score count, and tracking history of each play.
GAME MENU
When the first page loads, the user may choose to enter their name for tracking purposes, or the game will use “Unknown User” by default if the user chooses not to provide their name. 
The user can click on button “Play” to start Level 1.
The user can click button “Exit” to exit the game.
LEVEL 1
Page: 
-Score TextBlock (tracking how many points the user earned);
-Timer TextBlock (tracking countdown from 30 to 0);
-Missed Items TexBlock (tracking how many items the player missed);
-Player (represented by a big pot centered on the bottom of the page);
Game Rules:
The user has 30 seconds to collect as many items (falling mushrooms) as the can. They must use the Left/Right arrow kyes to move. To collect the item – the player must collide with it. If the user misses more than 3 items – they lose and they will be redirected to the Game Over Page.
If they win – the program will record their every play (username, date and time, the score) from each session and record into the game1_score.txt file.
If the user earns the highest score – they will get a congratulation message alert stating their score and the list of all players, game played and sorted by their scores. If they pass the level – the alert will notify their level and the list of players.
The user will be redirected to Level 2 once they complete 30 seconds without missing more than 3 items.
*Bonus Points: If the user collects 2 items at once – they will get 10 points, 3 and more – 20 points. (The challenge is to collect the items at the same time, the only option is to track the items which are vertically parallel to each other and to hit the center point between them).

LEVEL 2
Page: 
-Score TextBlock (tracking how many points the user earned);
-Timer TextBlock (tracking countdown from 30 to 0);
-Player (represented by a dog centered on the bottom of the page);

Game Rules:
The user has 30 seconds to avoid collision with the randomly created enemies coming from the top of the page. They must use the Left/Right arrow kyes to move. Once collision occurs – they loose and they will be redirected to the Game Over Page.
If they win – the program will record their every play (username, date and time, the score) from each session and record into the game2_score.txt file.
If the user earns the highest score – they will get a congratulation message alert stating their score and the list of all players, game played and sorted by their scores. If they pass the level – the alert will notify their level and the list of players.
The user will be redirected to Level 3 once they complete 30 seconds without hitting any enemy.
*Bonus Points: If the user collects 2 items at once – they will get 10 points, 3 and more – 20 points. (This is only based on luck how fast the enemies random spawn in the certain location).

LEVEL 3
Page: 
-Score TextBlock (tracking how many points the user earned);
-Timer TextBlock (tracking countdown from 30 to 0);
-Player (represented by a dog centered on the bottom of the page);



Game Rules:
The user has 30 seconds to avoid collision with the randomly created threats coming from the top of the page and they must collect the pumpkins. They must use the Left/Right arrow kyes to move. Once a collision occurs or if they miss 3 items – they lose and they will be redirected to the Game Over Page. 
If they win – the program will record their every play (username, date and time, the score) from each session and record into the game3_score.txt file.
If the user earns the highest score – they will get a congratulation message alert stating their score and the list of all players, game played and sorted by their scores. If they pass the level – the alert will notify their level and the list of players.
The user will be redirected to Finish Page once they complete 30 seconds without missing more than 3 items and hitting the threat.
*Bonus Points: If the user collects 2 items at once – they will get 10 points, 3 and more – 20 points. (The challenge is to collect the items at the same time, the only option is to track the items which are vertically parallel to each other and to hit the center point between them, as the items are randomly generated, this scenario is partially based on luck depending on where the items spawn).
GAME OVER PAGE
It stores and passes the username and gives the option to exit the game completely or to play the same level again.
FINISH PAGE
This Page notifies the user that they completed all levels and allow user to exit the game.
