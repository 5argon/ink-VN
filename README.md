# ink-VN

ink is a clean markup language that can create interactive story. It came without any implication about how you would use it in your projects. It's just that you can parse them line by line, or as much as possible until the branch.

Flexibility is great, but if you are planning to implement a visual novel from that flexibility in Unity, ink-VN might be of some use to you.

The target is to offer VN's common functions like [JokerScript](http://jokerscript.jp), but with less lock down on how to use and more DIY. JokerScript is like a complete end-to-end framework, while ink-VN is just a set of helpers. You must still code many implementations.

## Limitations

It will not support any kind of branching, since my game does not need them. Seems pretty critical to everyone else and ink is very strong for doing branches too. But that's open source projects, please contribute if you need it!

## License

Since the guys that created ink released it for the community for free, I also wanted to released ink-VN for free under the same license, MIT.

ink-VN does not include ink, but you will require ink to use ink-VN so please make sure to comply to ink's MIT license in your software too.

## Terms

- Talk Panel : The panel which displays the text that a character is talking.
- Advance : End the current talk panel and show the next one.
- Fast Forward : Skip the text reveal animation to the end.

## Main Concept : Animator

I will make it as flexible as possible. As such, I think Animator's trigger is a good entry way to almost everything.

This means if you want to make character change her face, if you want to fade the BG to black, etc. Whatever you want to do, do it in Animator's trigger. ink-VN can trigger those triggers and that's it! I believe in utilizing Unity's concept as much as possible. The animator will be commanded with ink's tags (#). The readed tags is just a string, and animator's trigger is based on string so I think this is perfect.

Animator can even change image's sprite so changing BG can be done in an Animator too! Let's do everything in an Animator. Because of this, I am not providing the command to change image, the command to fade out characters, etc. You must make them yourself in your Animator. Let's keep ink-VN minimal.

## Main Concept : DIY

ink-VN does not handle any screen resolution issue, does not care if your game is vertical or horizontal, etc. One thing you have to do is creating InkVNRunner game object and connect all the required components in the inspector, which you can build them however you like. (This is the point you have to make sure they are responsive, etc.)

You might like this approach if you are a person like me, who rather build and understand your own game to its fullest than take in too many unknowns at once from a big framework. It might be slower than a more pre-made frameworks but I think it saves time later on while debugging.

## Features 

The syntax is the same with ink's syntax plus the character names, and now certain tags (#) has meanings.

### Line

In one talk panel, it puts everything on the same line into it. When you advance the story by one step, it goes to the next non-empty line.

If you would like to put multiple lines in your ink script into one talk panel, you must use the tag `multi-lines [lines]`

### Text Reveal

The text can gradually reveal itself character-by-character as seen in many games. You can adjust its speed. I have solved the problem of line break for you too. If you just add one character over time normally when it reaches your Text's width a word might visibly jump down to the next line.

### Character Name

In ink-VN each line will looks like :

`5argon : Hello world!`

Which will results in character name "5argon" saying "Hello world!". This name can be paired with a color later so that it colorize the name.

There is an additional renaming syntax, which will looks like :

`Suspicious man (5argon) : Have you ever dream of making a game by yourself?`

The parenthesis can make it show up as "Suspicious man" in the game, but the name that will be used to pair with color will be "5argon". You can make it into ???, for example, if the character is initially unknown or not introduced his/her name yet.

### Tags

Functions will be mainly implemented using ink's tag. Tag sticks onto text in the same line or below the tag. So we can make something when the text is displayed. Some of the planned tags are :

```
//These 4 is the same thing which is triggering a trigger. The different name make it easier to read on your ink file.
//The trigger will in turn runs the animation. If you go to the next while the animation is unfinished, it will be fast-forwarded.
trigger [name] [triggerName]
image-trigger [name] [triggerName]
sprite-trigger [name] [triggerName]
character-trigger [name] [triggerName]

//This can redirect the text to elsewhere that is not the main text box.
//You might use it to make the text appear at center of the screen on black bg, for example.
//Automatically closes the main textbox.
alt-text [name]

//no effect until you implement the method. In case you use other music plugins or your own system. I use Introloop!
music-fade-out [seconds]
music-fade-in [name] [seconds]
sfx [name]

//Script does not know about length of your animations, so use wait for this
wait [seconds]

//Difference from wait is that this prevent the text box from closing in the time specified.
//Use it for long character animations
anti-skip [seconds]

//Merge the next n non-empty lines into the same talk panel.
multi-lines [n]
```

### Custom Tags

The tags provided might look lackluster. But the real power is that you can define your own tags in Unity that resolves into those basic tags. In my game I have `fade-out`, which resolves into `image-trigger blackImage fadeOutOneSecond` and `fade-out-end` which resolves into `image-trigger blackImage fadeOutLong`. It will be easier to understand in your ink script with custom tags.

I designed the framework to be minimal, so let's define what you want by yourself via custom tags.

## To Do

- I have to think of a way to do localization.
- Text reveal does not respect rich text, so if you have one the text will be stopped a moment when it reaches rich texts. (Since it is processing rich texts which are invisible.)

In development.
