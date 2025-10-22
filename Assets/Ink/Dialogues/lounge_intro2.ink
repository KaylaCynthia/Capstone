VAR nextChatArea = ""
VAR nextBranch = ""

# speaker: Player
# portrait: player_portrait
+[what's that?]

- # speaker: Ssunbite
# portrait: ssunbite_portrait
that's daily check in by our Rael
hi Rael <3

# speaker: Momeow
# portrait: momeow_portrait
omg Rael is here

# speaker: shadowReaper
# portrait: shadowreaper_portrait
another hard question

# speaker: Hneybee
# portrait: hneybee_portrait
ok but why did that hit harder than my morning coffee

# speaker: Rael
# portrait: rael_portrait
Hi everyone

# speaker: Sunny
# portrait: sunny_portrait
Hii Rael, this is Player_Name
the one i ask for permission earlier

# speaker: Rael
# portrait: rael_portrait
Oh, there you are
Everyone's been waiting to meet you, you know
Don't mind the noise, they just get excited when someone new shows up

# speaker: Player
# portrait: player_portrait
+[ah, so you are the one leading this little solar system] -> path1
+[it's fine, i don't mind a bit of chaos] -> path2

==path1==

# speaker: Rael
# portrait: rael_portrait
Haha, guilty as charged
Though leading might be a stretch
I just make sure that everyone can shine their light

# speaker: Ssunbite
# portrait: ssunbite_portrait
you're too modest, Rael
you're our sun

# speaker: HulioVerr
# portrait: hulioverr_portrait
even the new one gets it

# speaker: Ssunbite
# portrait: ssunbite_portrait
~ nextChatArea = "ChatAreaRael"
~ nextBranch = "rael_intro"
That's why he's different, okay?

->END

==path2==

# speaker: Rael
# portrait: rael_portrait
That's good to hear
Glad you're here to see what kind of mess we are

# speaker: C0deRuse
# portrait: coderuse_portrait
Careful, that's how it starts
next thing you know you're staying up at 3AM arguing about stars and fate

# speaker: Malides
# portrait: malides_portrait
~ nextChatArea = "ChatAreaRael"
~ nextBranch = "rael_intro"
true story btw

->END

