# rimworld-dps-calc
weapon dps calculator for rimworld CE guns

Doesn't factor armor penetration stuff.

Armor in CE works as layers, and each layer is checked against the armor pen value of an incoming projectile. Each layer the projectile penetrates reduces the damage of the projectile. Since this would be a pain in the ass to model, and relies on knowing the armor values of the layers.

I said nevermind all that and just multiplied the armor pen values by the base DPS to get an idea of what weapons might put out more overall penetration values per second.
