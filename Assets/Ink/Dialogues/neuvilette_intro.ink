# speaker: Neuvillette
# portrait: neuvillette_neutral
Greetings. I am Neuvillette, the Chief Justice of Fontaine. Your previous discussions with the gentlemen of Mondstadt and Liyue have been noted as... culturally informative.
The waters of conversation flow in interesting patterns. Tell me, what is your initial impression of Fontaine's judicial system?

+ [It seems very structured and formal.] -> neuvillette_structured
+ [I'm curious about how it all works.] -> neuvillette_curious
+ [It feels quite intimidating, honestly.] -> neuvillette_intimidating

=== neuvillette_structured ===
# speaker: Neuvillette
# portrait: neuvillette_thoughtful
Structure is necessary for justice to flow properly, much as riverbanks guide water. Without proper channels, truth becomes muddied.
The Palais Mermonia operates on centuries of legal precedent. Would you like to understand the foundational principles?

+ [Yes, please explain the foundations.] -> neuvillette_foundations_yes
+ [I'd prefer to hear about recent cases.] -> neuvillette_recent_cases
+ [How do you ensure fairness?] -> neuvillette_fairness

=== neuvillette_curious ===
# speaker: Neuvillette
# portrait: neuvillette_neutral
Curiosity is the first step toward understanding. Fontaine's legal system balances tradition with progressive thought.
Each judgment creates ripples that affect future proceedings. Which aspect of our jurisprudence interests you most?

+ [The role of evidence and proof.] -> neuvillette_evidence
+ [How judgments are delivered.] -> neuvillette_judgments
+ [The appeals process.] -> neuvillette_appeals

=== neuvillette_intimidating ===
# speaker: Neuvillette
# portrait: neuvillette_concerned
I understand the perception. The weight of justice can feel overwhelming to those unfamiliar with its workings.
However, the system exists to protect, not to oppress. What specific concerns would you like addressed?

+ [The formality of court proceedings.] -> neuvillette_formality
+ [The consequences of judgments.] -> neuvillette_consequences
+ [How citizens navigate the system.] -> neuvillette_navigation

=== neuvillette_foundations_yes ===
# speaker: Neuvillette
# portrait: neuvillette_thoughtful
The foundation rests on three pillars: Evidence must be as clear as mountain spring water, procedure must flow as predictably as the tides, and judgment must be impartial as rainfall.
These principles ensure that justice, like water, finds its proper level regardless of the vessel it fills.

VAR nextChatArea = "ChatAreaGeneral"
VAR nextBranch = "general_chat_start"

-> END

=== neuvillette_recent_cases ===
# speaker: Neuvillette
# portrait: neuvillette_neutral
A recent noteworthy case involved a clockwork meka accused of property damage. The complexity lay in determining intent versus programming error.
The judgment established new precedents for artificial intelligence in Fontaine law - a ripple that will influence future technological cases.

~ nextChatArea = "ChatAreaGeneral"
~ nextBranch = "general_chat_start"

-> END

=== neuvillette_fairness ===
# speaker: Neuvillette
# portrait: neuvillette_thoughtful
Fairness is maintained through multiple currents of oversight. Each case is reviewed by junior justices, then senior counsel, before reaching my bench.
Like multiple filters purifying water, this process removes bias and ensures only clear truth remains for final judgment.

~ nextChatArea = "ChatAreaGeneral"
~ nextBranch = "general_chat_start"

-> END

=== neuvillette_evidence ===
# speaker: Neuvillette
# portrait: neuvillette_neutral
Evidence must meet rigorous standards. Physical proof is examined as a geologist studies rock strata, witness testimony is weighed like precious gems, and documentation must be as consistent as the ocean's rhythm.
Without such standards, justice becomes as murky as silt-filled waters.

~ nextChatArea = "ChatAreaGeneral"
~ nextBranch = "general_chat_start"

-> END

=== neuvillette_judgments ===
# speaker: Neuvillette
# portrait: neuvillette_thoughtful
Judgments are delivered after careful contemplation of all currents. I consider not only the facts presented, but the broader implications for Fontaine's social waters.
Each verdict is crafted to restore balance, much as a skilled engineer might redirect water flow to prevent flooding.

~ nextChatArea = "ChatAreaGeneral"
~ nextBranch = "general_chat_start"

-> END

=== neuvillette_appeals ===
# speaker: Neuvillette
# portrait: neuvillette_neutral
The appeals process allows for reconsideration when new evidence emerges or procedural errors are discovered. Like water finding new paths around obstacles, the system adapts to ensure no injustice becomes permanent.
However, appeals require substantial grounds - we cannot have justice flowing in endless circles.

~ nextChatArea = "ChatAreaGeneral"
~ nextBranch = "general_chat_start"

-> END

=== neuvillette_formality ===
# speaker: Neuvillette
# portrait: neuvillette_concerned
The formality serves a purpose: it elevates the proceedings above petty disputes and reminds all participants of the gravity of justice.
Think of it as the ceremonial banks that contain a mighty river - they may seem restrictive, but they prevent chaos and destruction.

~ nextChatArea = "ChatAreaGeneral"
~ nextBranch = "general_chat_start"

-> END

=== neuvillette_consequences ===
# speaker: Neuvillette
# portrait: neuvillette_thoughtful
Consequences are measured carefully. Punishment should fit the transgression like a properly sized vessel contains water - neither overflowing nor insufficient.
Rehabilitation and restoration are always considered alongside retribution, for justice should heal as well as correct.

~ nextChatArea = "ChatAreaGeneral"
~ nextBranch = "general_chat_start"

-> END

=== neuvillette_navigation ===
# speaker: Neuvillette
# portrait: neuvillette_neutral
Citizens may seek guidance from legal aides at the Palais Mermonia, much as sailors consult navigation charts. The system, while complex, provides multiple channels for assistance.
No one should drown in legal complexities simply because they cannot afford expert representation.

~ nextChatArea = "ChatAreaGeneral"
~ nextBranch = "general_chat_start"

-> END