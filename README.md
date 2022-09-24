# 16 Bit RPG game
This Unity project is based on a Unity [tutorial][tutorial_link], and has been improved to include further features such as cutscenes, sound effects, particle effects and a dialogue system.

[tutorial_link]: https://www.youtube.com/watch?v=b8YUfee_pzc

The game has been built for WebGL and has been deployed here: https://jaaferh.github.io/DungeonUnityTut/

## Dialogue Options
The dialogue save file can be reset by clicking on the 'Reset Dialogue' button in the menu (accessible through the bottom left chest icon).

The dialogue system works by accepting a JSON list of NPC dialogue and another JSON list choicesChosen. NPC dialogue can contain keys which are added to the choicesChosen list once the player has gone through that dialogue or chosen an answer to a question.
The next lines of dialogue are chosen by priority of requirement matches with the keys stored in the choicesChosen list.

Details can be seen in the NPCTextPerson class under Scripts. NPC Dialogue JSON files can be viewed under JSON.