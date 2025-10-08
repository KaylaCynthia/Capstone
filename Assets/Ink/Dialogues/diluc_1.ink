VAR nextChatArea = ""
VAR nextBranch = ""

# speaker: Diluc
# portrait: diluc_thoughtful
I see you've concluded your discussions with our... esteemed acquaintances from Liyue and Fontaine. I trust their counsel was insightful, if perhaps a touch philosophical.

-> diluc_check_in

=== diluc_check_in ===
# speaker: Diluc
# portrait: diluc_neutral
My offer from earlier still stands, though its form can be adapted. Having spoken with them, the weight on your shoulders seems more defined, if not heavier. I find action often clarifies what words complicate.

+ [You're right. A drink/walk sounds better now.] -> diluc_renewed_offer
+ [It was... enlightening. I could use a distraction.] -> diluc_distraction
+ [They gave me a lot to think about. I need to be alone.] -> diluc_alone

=== diluc_renewed_offer ===
# speaker: Diluc
# portrait: diluc_happy
~ nextChatArea = "ChatAreaDiluc"
~ nextBranch = "next_day"
Good. I anticipated you might. I've taken the liberty of preparing the private room upstairs at the Angel's Share. No business, no grand theories—just a quiet moment. The grape juice is chilled, and I've even procured some non-alcoholic cider from Mondstadt's finest orchards, should you prefer a change.

-> END

=== diluc_distraction ===
# speaker: Diluc
# portrait: diluc_thoughtful
~ nextChatArea = "ChatAreaDiluc"
~ nextBranch = "next_day"
Understood. Sometimes the mind needs a rest. I was about to conduct a routine patrol along the Whispering Woods border. It is a simple, physical task that requires focus on one's immediate surroundings. The company would be welcome, and it is far removed from the burdens of archons and sovereigns.

+ [A patrol sounds perfect. Let's go.] -> diluc_patrol_yes
+ [I think I'll just take that grape juice at the tavern.] -> diluc_tavern_quiet

=== diluc_alone ===
# speaker: Diluc
# portrait: diluc_neutral
~ nextChatArea = "ChatAreaDiluc"
~ nextBranch = "next_day"
I respect that. Solitude can be the most effective counsel. Remember, the Winery and the tavern are sanctuaries should you need one. Do not stand on ceremony.

-> END

=== diluc_patrol_yes ===
# speaker: Diluc
# portrait: diluc_happy
~ nextChatArea = "ChatAreaDiluc"
~ nextBranch = "next_day"
Then it's settled. Meet me at the city's main gate in ten minutes. Bring your weapon. While the path is generally safe, it pays to be prepared. We can speak, or not speak, as you wish.

-> END

=== diluc_tavern_quiet ===
# speaker: Diluc
# portrait: diluc_thoughtful
~ nextChatArea = "ChatAreaDiluc"
~ nextBranch = "next_day"
A quiet corner is already reserved. I will instruct Charles that you are not to be disturbed. I will join you briefly to ensure you have what you need, but I will not impose. The space is yours for as long as you require it.

-> END