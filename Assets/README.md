// UI reference https://ashesofcreation.com/news/2023-02-025-development-update-with-ui-process-and-progress
// https://craftpix.net/product/rpg-game-ui/
// https://www.reddit.com/r/Smite/comments/11yymsy/ui_differences
// Selection circle
// https://krita-artists.org/t/how-would-you-go-about-drawing-this/49017/5
// TODO Dissolve 
// https://www.youtube.com/watch?v=taMp1g1pBeE
// healing spell effect
// https://www.youtube.com/watch?v=HdMdc850mhs
// UI references
// https://ashesofcreation.com/news/2023-02-025-development-update-with-ui-process-and-progress
// https://www.youtube.com/watch?v=o1SMcXbSY5k
// https://www.newworld.com/en-us/game/releases/june-part-1
// https://wago.io/OmmE6SX67
// AOE zones 
// https://www.google.com/imgres?q=aoe%20attacks%20game&imgurl=https%3A%2F%2Fffxiv.consolegameswiki.com%2Fmediawiki%2Fimages%2Fthumb%2Fd%2Fd8%2FLine_AoE1.jpg%2F300px-Line_AoE1.jpg&imgrefurl=https%3A%2F%2Fffxiv.consolegameswiki.com%2Fwiki%2FColumn_AoE&docid=JFZrHjmQTTo64M&tbnid=LxYAX0SJs5LZUM&vet=12ahUKEwj8irqM6OqFAxWU5skDHeCcB3IQM3oECGAQAA..i&w=300&h=380&hcb=2&ved=2ahUKEwj8irqM6OqFAxWU5skDHeCcB3IQM3oECGAQAA
// https://forum.unity.com/threads/a-comprehensive-guide-to-the-execution-order-of-unity-event-functions.1381647/

# TODO
Enemy hit boxes, instead of the center of them for the position, melee range on large mobs isnt working.
Using projectors/decals as targeting/summoning circles https://www.youtube.com/watch?v=oZX5UAru3CY
Spotlight selection circle https://www.youtube.com/watch?v=h-rTdArjyzU
selection circle on tarrain with shader https://www.youtube.com/watch?v=a-J4ktx6y0Y

Different style damage number https://www.google.com/search?client=firefox-b-1-d&sca_esv=b20e215ce61cde95&sca_upv=1&sxsrf=ADLYWIILxEeGuJoQSncg0HewkRoiz0D9XQ:1715016067053&q=unity+animated+floating+text+damage+number&tbm=vid&source=lnms&prmd=ivsnbmtz&sa=X&ved=2ahUKEwinwNfbxPmFAxUo4MkDHbfZDjEQ0pQJegQIDxAB&biw=1536&bih=703&dpr=1.25#fpstate=ive&vld=cid:4dcad031,vid:3eM-6qlwFuU,st:0
or 
https://www.youtube.com/watch?v=KOt85IoD__4

AOE Circles, using brakckeys shield video with this https://www.youtube.com/watch?v=S4W-QBpQgiU
render on both side and animate the edges. https://www.youtube.com/watch?v=tbySMSh6Ip0
Combination fresnal and a spotlight/decal
# Tasks:
Nameplate:
- [ ] Show nameplate when within x units of player
- [ ] Show only health of unity, level, name and status effects.,=
Unit Frame
- [ ] Targer frame, show mana if they ahve some
Ability types:
 - [ ] Melee attacks
 - - [X] Single target attack
 - - [X] Multiple swing attack
 - - [X] Multiple target attack
 - - [X] Whirlwind attack
 - - [X] Slowing attack
 - - [x] Stun attack
 - - [x] Bleeding sttack
 - - [X] Debuff attack
 - [ ] Ranged attacks
 - - [X] Single target attack
 - - - [X] Burn/Dot attack
 - - - [X] Casted ability
 - - - [X] instant cast
 - - - [ ] channeled ability
 - - [X] Multi target attack
 - - - [X] Multi projectile
 - - - [X] Splash attack
 - - - [X] Splash Burn attack
 - [ ] AOE Attack
 - - [X] Spot point AOE, Drop spot at location and it damages/heals all enemies/friendlies standing inside it.
 - - - [X] Attach spot to target and it moves with the target (New world beacon).
 - - - [X] Drop spot on self and it follows self.
 - - - [X] Drop spot at location and it stays there (New world sacred ground)
 - - - - When activating the ability, it changes the mouse cursor to the selection ring, when the mouse button is clicked it activated the spot aoe ability which spawns the aoe spot at that location.
 - - [ ] Channled spot AOE, while casting a spot point is active, when the casting is done the spot is destroyed (Blizzard, rain of fire)
 - - [ ] Explosion AOE, a blast of damage is sent off in a sphere damaging/healing all in radius, (wow arcane explosion, metoer splash damage, holy nova..)
 - - [ ] Line AOE, a line of damaging/healing spot is placed in a line in front of the caster, all player inide this line are effected
 - - [ ] ORB AOE, orb is sent out in a direction for x second, whoever it hits damages/heals
 - - [ ] Shape based AOE, trapazoid/cone based attacks that effect all players in the area.
UI
- [ ] Range based enemy nameplate
- [ ] stacking nameplates