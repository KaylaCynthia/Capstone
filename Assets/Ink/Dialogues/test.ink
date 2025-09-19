# speaker: Person One
# portrait: person_one
(You saw a person online)
heheheuawheflwheeuwaifhlwefawleiufweajfweliuajweilfnalwiufhilwaeuhni.gubesinhaewcoeklfiaweufnewuonveuawifnwepofjlhgaeuwfjewuifhniluwehfilawehnfiluwaenhliufhnawiweuhleaiwufhiweulfhaliwehfilawuehfnilewhnfaliwehfnuylwbglubaweofewuafepwjeoawjiulewanliugvawenhgiewfjewoifaewjofwaeliujfoamidsuncianeifhawougnviueaswnweofjwoaefnwileuafnieusnvauoewijfpoeifmawpoeofawneofnaweofnwaoeucewiopjfoaweinmcawoenfowfjweoafmaowiemnfvoaiwemoewaifewoaicmoewaifnoiawnecowaeoijfawiemcoiaweoweapf

-> next_message

=== next_message ===
# speaker: Person Two
# portrait: person_two
Where am I?...

+ [It's going great!] -> great_day
+ [Could be better...] -> bad_day  
+ [Just average] -> average_day

-> DONE

=== great_day ===
# speaker: Person Two  
# portrait: person_two
That's wonderful to hear! Want to grab coffee sometime?

+ [Sure, I'd love to!] -> coffee_yes
+ [I'm not really a coffee person] -> coffee_no
+ [Maybe another time] -> coffee_later

-> DONE

=== bad_day ===
# speaker: Person Three
# portrait: person_three
Oh no, sorry to hear that. Want to talk about it?

+ [Yes, I could use someone to talk to] -> talk_yes
+ [No, I prefer to keep to myself] -> talk_no  
+ [Maybe later, thanks] -> talk_later

-> DONE

=== average_day ===
# speaker: Person Four
# portrait: person_four
Sometimes average days are the most peaceful! Any plans for the weekend?

+ [Going hiking] -> plans_hiking
+ [Probably just relaxing] -> plans_relax
+ [Not sure yet] -> plans_unknown

-> DONE

=== coffee_yes ===
# speaker: Person One
# portrait: person_one
Great! How about Saturday at 2 PM?

+ [Perfect!] -> confirm_coffee
+ [Can we make it Sunday?] -> change_day
+ [Let me check my schedule] -> check_schedule

-> DONE

=== coffee_no ===
# speaker: Person Five
# portrait: person_five
No problem! Maybe we could do something else instead?

-> END

=== coffee_later ===
# speaker: Person Two
# portrait: person_two
No worries! The offer stands anytime.

-> END

=== talk_yes ===
# speaker: Person Three  
# portrait: person_three
I'm here to listen. What's been bothering you?

-> END

=== talk_no ===
# speaker: Person Four
# portrait: person_four
I understand. Hope your day gets better!

-> END

=== talk_later ===
# speaker: Person One
# portrait: person_one
Okay, just know I'm here if you need me.

-> END

=== plans_hiking ===
# speaker: Person Five
# portrait: person_five
That sounds fun! Which trail are you thinking?

-> END

=== plans_relax ===
# speaker: Person Two
# portrait: person_two
Nothing wrong with some good rest! Enjoy your weekend.

-> END

=== plans_unknown ===
# speaker: Person Three
# portrait: person_three
Well, I hope you find something enjoyable to do!

-> END

=== confirm_coffee ===
# speaker: Person One
# portrait: person_one
Awesome! See you Saturday at 2 PM

-> END

=== change_day ===
# speaker: Person Four
# portrait: person_four
Sunday works too! Same time?

-> END

=== check_schedule ===
# speaker: Person Five
# portrait: person_five
No rush! Just let me know when you know.

-> END