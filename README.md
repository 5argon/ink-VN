# ink-VN

ink is a clean markup language that can create interactive story. It came without any implication about how you would use it in your projects. It's just that you can parse them line by line, or as much as possible until the branch.

Flexibility is great, but if you are planning to implement a visual novel from that flexibility in Unity, ink-VN might be of some use to you.

The target is to offer VN's common functions like [JokerScript](http://jokerscript.jp), but with less lock down on how to use and more DIY. JokerScript is like a complete end-to-end framework, while ink-VN is just a set of helpers. You must still code many implementations.

Functions will be mainly implemented using ink's tag. Tag sticks onto text in the same line or below the tag. So we can make something when the text is displayed. Some of the planned tags are :


```
image-color [name] [(r,g,b,a)]
sprite-color [name] [(r,g,b,a)]
image-change [name] [spriteName]
sprite-change [name] [spriteName]
animator-trigger [name] [triggerName]

character-show [name] [(positionSyntax)] [facing]
character-hide [name] [(positionSyntax)]
character-facing [name] [facing]
character-move [name] [(positionSyntax)] [fadeOut/fadeIn]
character-exp [name] [triggerName] //same effect as animator-trigger

music-fade-out [seconds]
music-fade-in [name] [seconds]
sfx [name]
wait [seconds]
```

And a way of doing localization.

In development.

