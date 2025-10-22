VAR nextChatArea = ""
VAR nextBranch = ""

# speaker: Rael
# portrait: rael_portrait
Hey, didn't mean to pull you away
Just wanted a quieter space to say hi
Also i wanted to check in, make sure you're not overwhelmed or anything

# speaker: Player
# portrait: player_portrait
it's fine. everyone seems nice
+[a little obsessed with you though.] -> path1
+[feels like you've got your own fanclub in there] -> path2

==path1==
# speaker: Rael
# portrait: rael_portrait
Oh? Is that how it seems?
I won't really said obsessed
They always tell me they’re grateful for me, and somehow, they just naturally end up following me.
But i never wanted them to feel that way
I just wanted to help others and make a difference

->continue

==path2==
# speaker: Rael
# portrait: rael_portrait
Haha, something like that
I promise i never ask for fan club status

# speaker: Player
# portrait: player_portrait
+[must be a lot pressure, huh?]

- # speaker: Rael
# portrait: rael_portrait
It can be, yeah
it's all felt worth it the moment I realized I could make a difference for someone else

# speaker: Player
# portrait: player_portrait
+[i see...]

->continue

==continue==

# speaker: Player
# portrait: player_portrait
+[that's.... actually kind of sweet]

- # speaker: Rael
# portrait: rael_portrait
Thanks for saying that, really
i forget sometimes that what i did actually reaches people

# speaker: Player
# portrait: player_portrait
+[i get that]
- on the internet, it's easy to feel like you're just talking into the void sometimes

# speaker: Rael
# portrait: rael_portrait
Exactly
That's why i kinda like seeing faces, you know?
Makes it feel real
Not just words floating around

# speaker: Player
# portrait: player_portrait
+[You mean like, video chat?]

- # speaker: Rael
# portrait: rael_portrait
if, you're comfortable with it
No pressure at all
Just thought it's be nice to talk without all the text in between
You can even keep the camera off if you want, i don't mind

# speaker: Player
# portrait: player_portrait
+[you do that with everyone here?]

- # speaker: Rael
# portrait: rael_portrait
Pretty much yeah
It's how the group started, actually
Just people talking, finding a bit of warmth in a study group
Feels different when it's not just text you know?

# speaker: Player
# portrait: player_portrait
+[it's kinda late now, tho]
- Maybe tomorrow?

# speaker: Rael
# portrait: rael_portrait
~ nextChatArea = "ChatAreaRael"
~ nextBranch = "next_day"
No rush
I just think it'd be nice to actually see you
Anyway, get some rest. yeah?
The world's brighter when you're not running on fumes

->END