**My custom commands made to be used for my game development called Aya's Loop**  
<hr>
PortraitAnime - Portrait frame-by-frame animation (EXPERIMENTAL!). Copy this to assets, and PortraitAnimEditor to Editor folder.
<hr>
BaloonDialog -  Baloon Thoughts/Dialog like in anime/manga  
<hr>
Clickable Character - You must eyeballing the position of the BoxCollider on Scene while the game is running to get the exact position.  
Copy ClickableCharacterEditor.cs to Editor folder  
Copy ClickableCharacter.cs to your Assets folder
<hr>  
CartoonEffects - (EXPERIMENTAL) 2D animation effects  
<hr>
PauseScreen - Pause command  
<hr>
ThreeFramer - Frame-by-frame background animation  
Making backgound to move in loop by continuously shuffling it's index.  
Note: This is single instance only. Meaning it can't animate multiple backgound instances!  
<hr>
Transition - Custom seamless Transition animation with jump through label feature.  
<hr>
twoDeffects - Fake 2D particle system through LeanTween.  
<hr>
PortraitAnim - Character frame-by-frame animation utilizing the portraits list(use Enable & Disable to activate and deactivate)  
Making a character blinking, noding or moving in general pretty much easier.
Access from code : PortraitAnim varname = GetComponent<PortraitAnim>(); varname.disablePortraitAnim(false);  
Note: You must copy the PortraitAnimEditor to Editor folder!  
<hr>  
Say3rd - Say command with 3rd person/narrator functionality like in story books  
<hr>  
SetMouseCursor - Custom cursor with animation while clicking.  
<hr>
BetterWait - To halt execution temporarily with click spam prevention, which the original doesn't have. The original uses Invoke, this uses Coroutine.  
<hr>
StandAloneCharacterPointSystem - Super simple realtime variable loader that can be used for; affection point/MP/HP. Put this on separate/disconnected block  
