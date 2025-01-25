INCLUDE GlobalNOTSTORY.ink

{ NAME == "": -> main | -> already }

=== main ===
HEY!#speaker:???#portrait:StickMAn#layout:Right
What's your nickname?#speaker:???#portrait:StickMAn#layout:Right
    +[Jeff]
        Jeff?
        My Name is Jeff!#speaker:Jeff#portrait:StickMAn#layout:Right
        -> chosen("Also Jeff")
    +[DirtEater54]
        You really think i'm that stupid.
        You're name is Stupid!
        -> chosen("Stupid")
    +[I Don't Care]
        Hmm...
        You're no fun.
        OK, "I Don't Care".
        -> chosen("I Don't Care")
=== chosen(name) ===

~ NAME = name
-> END

=== already ===
Hello, I know your name!
-> END