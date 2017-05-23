# ink-VN

ink is a clean markup language that can create interactive story. It came without any implication about how you would use it in your projects. It's just that you can parse them line by line, or as much as possible until the branch.

Flexibility is great, but if you are planning to implement a visual novel from that flexibility in Unity, ink-VN might be of some use to you.

The target is to offer VN's common functions like [JokerScript](http://jokerscript.jp), but with less lock down on how to use and more DIY. JokerScript is like a complete end-to-end framework, while ink-VN is just a set of helpers. You must still code many implementations.

## Main Concept : Animator

I will make it as flexible as possible. As such, I think Animator's trigger is a good entry way to almost everything.

This means if you want to make character change her face, if you want to fade the BG to black, etc. Whatever you want to do, do it in Animator's trigger. ink-VN can trigger those triggers and that's it! I believe in utilizing Unity's concept as much as possible. 

Animator can even change image's sprite so changing BG can be done in an Animator too! Let's do everything in an Animator. Because of this, I am not providing the command to change image, the command to fade out characters, etc. You must make them yourself in your Animator. Let's keep ink-VN minimal.

## Main Concept : DIY

ink-VN does not handle any screen resolution issue, does not care if your game is vertical or horizontal, etc. One thing you have to do is creating InkVNRunner game object and connect all the required components in the inspector, which you can build them however you like. (This is the point you have to make sure they are responsive, etc.)

You might like this approach if you are a person like me, who rather build and understand your own game to its fullest than take in too many unknowns at once from a big framework. It might be slower than a more pre-made frameworks but I think it saves time later on while debugging.

## What I will not implement

It will not support any branching, since my game does not need it. Seems pretty critical to everyone else! But that's open source projects, please contribute if you need it!

## How

Functions will be mainly implemented using ink's tag. Tag sticks onto text in the same line or below the tag. So we can make something when the text is displayed. Some of the planned tags are :

```
//These 3 is the same thing which is triggering a trigger. The different name make it easier to read on your ink file.
image-trigger [name] [triggerName]
sprite-trigger [name] [triggerName]
character-trigger [name] [triggerName]

//no effect until you implement the method. In case you use other music plugins or your own system. I use Introloop!
music-fade-out [seconds]
music-fade-in [name] [seconds]
sfx [name]
wait [seconds]
```

And a way of doing localization.

In development.
