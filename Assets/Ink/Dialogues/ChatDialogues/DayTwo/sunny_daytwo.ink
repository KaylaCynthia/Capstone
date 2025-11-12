VAR nextChatArea = ""
VAR nextBranch = ""

# speaker: Sunny
# portrait: sunny_portrait
hey there
how's your impression on the server?

# speaker: Player
# portrait: player_portrait
+[it felt really great tbh :D]
- i didn't expect that they would be very welcoming

# speaker: Sunny
# portrait: sunny_portrait
that's awesome!
btw
becareful not to get addicted with this server

# speaker: Player
# portrait: player_portrait
+[are you teasing me rn?] -> normal
+[hey, i'm not that into Rael] -> intorael

==normal==
# speaker: Sunny
# portrait: sunny_portrait
dont get me wrong, i just want to make sure you're falling too fast haha

# speaker: Player
# portrait: player_portrait
+[skeptical eh]

- # speaker: Sunny
# portrait: sunny_portrait
hehehe

->continuestory

==intorael==
# speaker: Sunny
# portrait: sunny_portrait
i mean... i can see the signs XD
but alright i won't budge you anymore

->continuestory

==continuestory==
# speaker: Sunny
# portrait: sunny_portrait
seems like they're starting to get active too at the server
see you there ig

# speaker: Player
# portrait: player_portrait
+[oh yeah? aight i suppose i'll see u there]
+[lmao let's see what they're cooking]

- # speaker: Sunny
# portrait: sunny_portrait
probably something silly or enlightening as usual
~ nextChatArea = "ChatAreaLounge"
~ nextBranch = "lounge_daytwo"
cool.

-> END