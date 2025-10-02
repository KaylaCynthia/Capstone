VAR nextChatArea = ""
VAR nextBranch = ""

# speaker: Somebody
# portrait: Somebody_hehe
(Diluc Went Online)
(Just reading the text messages while scrolling on tiktok on the other half of the screen, wondering what the group is so excited in talking about)

-> diluc_conversation

=== diluc_conversation ===
# speaker: Diluc
# portrait: diluc_neutral
Ah, you're online. I noticed you haven't been at the Angel's Share recently. I trust everything is... manageable?
The tavern has been unusually quiet without your occasional presence.

+ [Things have been hectic, but I'm okay.] -> diluc_busy
+ [Honestly, it's been a rough week.] -> diluc_rough_week
+ [Just the usual routine, nothing special.] -> diluc_routine

=== diluc_busy ===
# speaker: Diluc
# portrait: diluc_thoughtful
I see. The burdens of responsibility are not unfamiliar to me. It is important to find moments of respite, lest one burns out entirely.
In fact, I was about to take a brief respite from the winery's paperwork. Would you care to join me for a drink? I can assure you, the grape juice I keep in stock is of the highest quality.

+ [I'd love to join you! Grape juice sounds perfect.] -> diluc_juice_yes
+ [I appreciate it, but I'm not really thirsty.] -> diluc_juice_no
+ [Maybe later? I'm still sorting a few things out.] -> diluc_juice_later

=== diluc_rough_week ===
# speaker: Diluc
# portrait: diluc_concerned
...I see. While I am not one for empty platitudes, know that you have... an ally. Should you need to speak of it, or if the source of your troubles requires a more... direct solution, my resources are at your disposal.
A change of scenery often helps. Allow me to offer you one. There's a small, quiet place I know that serves excellent tea.

+ [Thank you, Diluc. Talking would be nice.] -> diluc_talk_yes
+ [I'll handle it, but I appreciate the offer.] -> diluc_talk_no
+ [Tea sounds peaceful. Let's do that.] -> diluc_tea_yes

=== diluc_routine ===
# speaker: Diluc
# portrait: diluc_neutral
Routine provides a necessary structure. There is merit in a predictable, peaceful existence. One might even call it a luxury.
That said, even the most well-oiled machinery requires maintenance. I was planning to inspect the vineyards this afternoon. The evening light over the slopes is quite a sight. It's a practical matter, but the company would not be unwelcome.

+ [A walk in the vineyards sounds wonderful.] -> diluc_walk_yes
+ [I should probably stick to my own schedule today.] -> diluc_walk_no
+ [Can I let you know a bit later?] -> diluc_walk_later

=== diluc_juice_yes ===
# speaker: Diluc
# portrait: diluc_happy
~ nextChatArea = "ChatAreaZhongli"
~ nextBranch = "zhongli_intro"
Good. I will meet you at the Angel's Share in one hour. The main hall is closed to patrons at this time, so we will not be disturbed. I have a new batch from the southern vineyards I've been meaning to sample. Your opinion would be... valued.
-> DONE

=== diluc_juice_no ===
# speaker: Diluc
# portrait: diluc_neutral
Very well. The offer remains open. Do not hesitate to stop by if your duties lighten. I will ensure a bottle is set aside for you.

~ nextChatArea = "ChatAreaZhongli"
~ nextBranch = "zhongli_intro"

-> END

=== diluc_juice_later ===
# speaker: Diluc
# portrait: diluc_thoughtful
Understood. I will be occupied with inventory until nightfall. Simply send a message when you are available. I will see if my schedule permits.

~ nextChatArea = "ChatAreaZhongli"
~ nextBranch = "zhongli_intro"

-> END

=== diluc_talk_yes ===
# speaker: Diluc
# portrait: diluc_concerned
Then I am listening. You can speak freely; these messages are secure. Whether it is a matter of the heart or a more tangible problem, we will address it. Please, begin whenever you are ready.

~ nextChatArea = "ChatAreaZhongli"
~ nextBranch = "zhongli_intro"

-> END

=== diluc_talk_no ===
# speaker: Diluc
# portrait: diluc_neutral
Your resolve is noted. I will not press the matter further. However, my earlier statement stands. Should the situation change, you know where to find me. Stay vigilant.

~ nextChatArea = "ChatAreaZhongli"
~ nextBranch = "zhongli_intro"

-> END

=== diluc_tea_yes ===
# speaker: Diluc
# portrait: diluc_happy
A sound decision. The place is a small house near the city's secondary gate, marked by a pot of glaze lilies. It is discreet. I will procure a table for us in thirty minutes. They serve a Sumeru-inspired blend that is particularly effective at calming the nerves.

~ nextChatArea = "ChatAreaZhongli"
~ nextBranch = "zhongli_intro"

-> END

=== diluc_walk_yes ===
# speaker: Diluc
# portrait: diluc_happy
Excellent. It will be a productive outing. Meet me at the winery's main entrance. And dress appropriately; the paths can be muddy depending on the recent rainfall. I will have a spare cloak if required.

~ nextChatArea = "ChatAreaZhongli"
~ nextBranch = "zhongli_intro"

-> END

=== diluc_walk_no ===
# speaker: Diluc
# portrait: diluc_neutral
As you wish. Adherence to one's schedule is a virtue. Should you find your routine leading you near the Dawn Winery, do not consider it an intrusion to stop by.

~ nextChatArea = "ChatAreaZhongli"
~ nextBranch = "zhongli_intro"

-> END

=== diluc_walk_later ===
# speaker: Diluc
# portrait: diluc_thoughtful
I will be on the grounds until sunset. Inform Adelinde, the head maid, if you decide to come. She will know where to find me. There is no pressure either way.

~ nextChatArea = "ChatAreaZhongli"
~ nextBranch = "zhongli_intro"

-> END