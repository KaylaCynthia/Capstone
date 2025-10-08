VAR nextChatArea = ""
VAR nextBranch = ""
# speaker: Diluc
# portrait: diluc_sleepy
(The message notification chimes unusually early. The portrait that appears is of Diluc, his hair slightly less perfectly coiffed than usual, a simple ceramic mug of black coffee in his hand. The background is a sunlit window overlooking the Dawn Winery vineyards.)
Good morning. I trust you rested adequately.

-> diluc_morning_offer

=== diluc_morning_offer ===
# speaker: Diluc
# portrait: diluc_neutral
Adelinde has prepared a modest breakfast. The morning air in the vineyards is crisp and carries the scent of damp earth and grapes. It is... a fresh start.
I find myself with a clear schedule this morning before the day's responsibilities begin.

+ [Good morning! A walk sounds lovely.] -> diluc_morning_walk
+ [I'm still waking up... Is there more coffee?] -> diluc_morning_coffee
+ [What's on the agenda for today?] -> diluc_morning_agenda

=== diluc_morning_walk ===
# speaker: Diluc
# portrait: diluc_happy
~ nextChatArea = "ChatAreaZhongli"
~ nextBranch = "zhongli_intro"
A wise choice. I will wait for you on the terrace. We can survey the lower slopes. The falcons are particularly active at this hour. It is a good time to ensure the perimeter is secure, while also... appreciating the calm.

-> END

=== diluc_morning_coffee ===
# speaker: Diluc
# portrait: diluc_thoughtful
~ nextChatArea = "ChatAreaZhongli"
~ nextBranch = "zhongli_intro"
There is always more coffee. The Winery stocks a robust blend from Sumeru that is highly effective. I will have a pot sent to the sunroom. Join me when you are more... cognizant. The quiet there is agreeable.

-> END

=== diluc_morning_agenda ===
# speaker: Diluc
# portrait: diluc_neutral
~ nextChatArea = "ChatAreaZhongli"
~ nextBranch = "zhongli_intro"
Firstly, peace. Then, a review of the ledgers. After that, I am needed at the Angel's Share to receive a new shipment.
However, the latter can be delegated if a more... engaging opportunity arises. Did you have something in mind?

+ [Let's just enjoy the peace for now.] -> diluc_enjoy_peace
+ [A new shipment? Can I help?] -> diluc_help_shipment
+ [I was thinking of exploring Wolvendom...] -> diluc_explore

=== diluc_enjoy_peace ===
# speaker: Diluc
# portrait: diluc_happy
~ nextChatArea = "ChatAreaZhongli"
~ nextBranch = "zhongli_intro"
An excellent agenda. I will inform Adelinde to hold all non-urgent matters. The terrace is yours. I will be nearby, should you wish for company or conversation.

-> END

=== diluc_help_shipment ===
# speaker: Diluc
# portrait: diluc_thoughtful
~ nextChatArea = "ChatAreaZhongli"
~ nextBranch = "zhongli_intro"
Your assistance would be... efficient. It involves inspecting crates and confirming inventory. Not the most thrilling task, but it is honest work. We could proceed to the city together after breakfast.

-> END

=== diluc_explore ===
# speaker: Diluc
# portrait: diluc_concerned
~ nextChatArea = "ChatAreaZhongli"
~ nextBranch = "zhongli_intro"
Wolvendom? That is not a casual morning stroll. The terrain is rough, and the local wolf packs, while generally avoiding humans, are unpredictable.
...Very well. But we go prepared. I will bring my sword, and you will stay close. We can depart after we've both eaten a proper meal.

-> END