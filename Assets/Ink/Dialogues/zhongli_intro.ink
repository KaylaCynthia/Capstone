# speaker: Zhongli
# portrait: zhongli_neutral
Ah, we meet again. I hope your previous conversation with Master Diluc was... fruitful. 
The threads of fate often bring interesting constellations of people together. Tell me, how has your journey through Liyue's landscapes been treating you lately?

+ [Liyue's scenery is absolutely breathtaking!] -> zhongli_scenery
+ [It's been challenging, but rewarding.] -> zhongli_challenging
+ [I'm still getting used to the culture here.] -> zhongli_culture

=== zhongli_scenery ===
# speaker: Zhongli
# portrait: zhongli_happy
Indeed. The stone forests of Guyun, the bustling harbor, the serene Qingce Village - each tells a story millennia in the making. 
The very mountains remember contracts made before your civilization began. Would you be interested in hearing one such tale?

+ [I'd love to hear an ancient story!] -> zhongli_story_yes
+ [Maybe another time, but thank you.] -> zhongli_story_no
+ [Could we discuss something more current?] -> zhongli_current

=== zhongli_challenging ===
# speaker: Zhongli
# portrait: zhongli_thoughtful
Challenge tempers the spirit much as fire tempers steel. Liyue's trials are designed to reveal one's true character.
The adepti themselves underwent similar trials in ages past. Would you like to know how they overcame their obstacles?

+ [Yes, I need some inspiration!] -> zhongli_adepti_yes
+ [I prefer to find my own way.] -> zhongli_adepti_no
+ [Maybe some practical advice instead?] -> zhongli_advice

=== zhongli_culture ===
# speaker: Zhongli
# portrait: zhongli_neutral
Understanding comes with time, like water smoothing stone. Liyue's traditions run deep, rooted in contracts and commerce.
The Lantern Rite, the Rite of Descension, business negotiations - all follow established patterns. Which aspect intrigues you most?

+ [The festivals and celebrations!] -> zhongli_festivals
+ [The business and contract systems.] -> zhongli_contracts
+ [The history behind it all.] -> zhongli_history

=== zhongli_story_yes ===
# speaker: Zhongli
# portrait: zhongli_happy
Excellent. This tale concerns the formation of the Guyun Stone Forest - not merely natural formations, but the very stone spears I once cast down during ancient conflicts.
Each pinnacle stands as a monument to a contract fulfilled, a threat neutralized, a balance maintained...

VAR nextChatArea = "ChatAreaNeuvillette"
VAR nextBranch = "neuvillette_intro"

-> END

=== zhongli_story_no ===
# speaker: Zhongli
# portrait: zhongli_neutral
As you wish. Some stories are better appreciated when one is truly ready to hear them.
The stones will keep their secrets until you are prepared to listen. There is wisdom in knowing when to wait.

~ nextChatArea = "ChatAreaNeuvillette"
~ nextBranch = "neuvillette_intro"

-> END

=== zhongli_current ===
# speaker: Zhongli
# portrait: zhongli_thoughtful
Current affairs, while fleeting, have their own significance. The recent developments in Fontaine's technology, for instance, present interesting challenges to traditional Liyue business practices.
Even eternal stones must adapt to changing tides.

~ nextChatArea = "ChatAreaNeuvillette"
~ nextBranch = "neuvillette_intro"

-> END

=== zhongli_adepti_yes ===
# speaker: Zhongli
# portrait: zhongli_happy
Then listen closely. The adepti learned that true strength comes not from power alone, but from understanding one's place in the natural order.
Moon Carver once spent three centuries contemplating a single mountain peak before understanding its essence. Patience reveals what force cannot.

~ nextChatArea = "ChatAreaNeuvillette"
~ nextBranch = "neuvillette_intro"

-> END

=== zhongli_adepti_no ===
# speaker: Zhongli
# portrait: zhongli_neutral
A respectable position. Forging one's own path has its own merits. Just remember that even the tallest mountains were once mere sediment, shaped by time and pressure.
Your journey will shape you in ways you cannot yet imagine.

~ nextChatArea = "ChatAreaNeuvillette"
~ nextBranch = "neuvillette_intro"

-> END

=== zhongli_advice ===
# speaker: Zhongli
# portrait: zhongli_thoughtful
Practical advice, then: Observe the merchants in Yujing Terrace. Notice how they negotiate not just for profit, but for long-term relationships.
In Liyue, a well-honored contract is more valuable than immediate gain. This principle serves well in all aspects of life.

~ nextChatArea = "ChatAreaNeuvillette"
~ nextBranch = "neuvillette_intro"

-> END

=== zhongli_festivals ===
# speaker: Zhongli
# portrait: zhongli_happy
The festivals are the living heartbeat of Liyue. Each lantern released during the Lantern Rite carries hopes and dreams to the heavens.
The flavors, the sounds, the collective joy - these moments define a people as much as their laws and commerce.

~ nextChatArea = "ChatAreaNeuvillette"
~ nextBranch = "neuvillette_intro"

-> END

=== zhongli_contracts ===
# speaker: Zhongli
# portrait: zhongli_neutral
Ah, the foundation of civilization. In Liyue, a contract is more than mere words - it is a bond that shapes reality itself.
From the simplest trade agreement to the most complex business venture, all operate within this sacred framework.

~ nextChatArea = "ChatAreaNeuvillette"
~ nextBranch = "neuvillette_intro"

-> END

=== zhongli_history ===
# speaker: Zhongli
# portrait: zhongli_thoughtful
History provides context for the present. The very ground beneath Liyue Harbor remembers when it was but a small fishing village.
Understanding where we came from illuminates where we might go. The past is never truly past in a nation as ancient as this.

~ nextChatArea = "ChatAreaNeuvillette"
~ nextBranch = "neuvillette_intro"

-> END