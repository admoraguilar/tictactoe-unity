FOX CUB GAMES DEV TEST - TIC TAC TOE


Developer: Admor Aloysious Aguilar
Email: admor.aguilar@gmail.com
Site: https://www.wishfuldroplet.com/
LinkedIn: https://www.linkedin.com/in/admoraguilar/


NOTES:
* This project is structured in a "package-based" way. For example for the scene called "MainScreen", everything that
  relates to that is contained in _TicTacToe/GameStates/MainScreen. For something that is shared across packages we
  put them in a folder one level above those packages called "_Common".
* Most game logic for the TicTacToe are under "_TicTacToe/GameStates/TicTacToeScreen".
* There's alot of areas that could be improved regarding the architecture of the game and some parts of the game logic.
  But due to time constraints, for some implementations I've opted for the simpler and faster way 
  but is still okay and gets the job done. 
* If you have any questions you can always email me at: admor.aguilar@gmail.com
* I can also be at Skype: admor.aguilar
* I also thank you for taking your valuable time to review this test.


Tic Tac Toe is a simple game played by 2 players on a square grid of tiles:
✓ Each player has a different symbol (traditionally X or O, but here we have custom symbol images).
   * In TicTacToeUI component we could specify "markers/symbols" for players.


✓ When the game start, each tile is empty.
   * At start, the tiles are empty.
   * After a player wins and they decide to play again, the tiles are emptied.
   * This could be observed by inspecting the TicTacToeGame and TicTacToeUI component. Those two are separated
     because the game logic should never know about the UI. This also allows us to swap different UIs to draw
	 the game logic if we want to.


✓ Players take turns selecting an empty tile and placing their symbol in that tile.
   * If a tile is also filled, a debug prompt is shown saying that the tile is filled.


✓ The game is over when a single row, column, or diagonal is completely filled with one Player's symbol
   * In TicTacToeWinCombinationData you can specify what combinations shall a player need to do to win.
   * In _TicTacToe/GameStates/TicTacToeScreen/ScriptableObjects you could find win combination data for different
     board sizes, you could also add a custom combination on how to win the board.


✓ The player should pick the grid size at the start of the game: 3x3 or 4x4.
   * This could be seen at the MainScreen scene where the player could choose between the two.


✓ No AI is necessary, both players' turns can be controlled by the same mouse input.


✓ We also want to store, in memory, a history of every move played in the game. 
   We don't need to do anything with this data, just store it. 
   Choose the correct data structure for the job. 
   * On the TicTacToeScreen scences, there are GameObjects under Systems that has a TicTacToeGameAnalytics component.
   * There we track what kind of game is played, who won the game, and the moves the player did for a game.

   
✓ The game doesn't need to look amazing, but it should look good, even with simple assets.
   Most importantly, the user experience should be solid.
   * For the UX, there are states for a play:
     * Splash - shows a logo
	 * Main - shows available tic tac toe board sizes
	 * TicTacToe -
		* Play - when the player clicks on a tile their symbol is put in it.
		* Win - shows an overlay showing who won the game, also has buttons for "Play Again", or "Back to Menu"
		* Draw - shows an overlay showing that the game is draw, also has buttons for "Play Again", or "Back to Menu"