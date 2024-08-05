## Project Overview

### Characters

Players and enemies are made as same Character objects, which acts as a facade to other systems interacting with them. Both players and enemies have different components attached, composing the final behavior.
- Shared: Health, ModifierHolder
- Player: Controller, WeaponUser, LevelingSystem, and UpgradeHolder.
- Enemies: AIController, LootDropper, and DamageOnCollision.

It is possible to create special enemies that use weapons by adding a WeaponUser component to them.\
Both of them have their initial Stats and StartingWeapons configured in ScriptableObjects.

### Weapons

I aimed to make weapons modular to support various behaviors, not only shooting.
To achieve this, I created weapons and actions they perform as separate configurable ScriptableObjects.
Every action inherits from one abstract WeaponAction class and implemnts its own Execute method.\
Weapon data holds a list of actions to execute on use.
- The implemented actions include CreateProjectile, DealDamage, and ApplyModifier.

WeaponActions may also have requirements for execution and is checked if the character using that weapon meets them. This is used to unlock poison and freeze modifiers for projectiles.

In the case of ranged weapons, they all have a CreateProjectile action, which itself has a list of actions it passes to the created projectile to execute upon hitting a target.

### Modifiers

Modifiers are also implemented as ScriptableObjects and can be configured to create their behavior. They use WeaponActions as well and can have a Ticker to execute them on every tick (for damage over time effects)
They can also have stat modifiers and apply them to target (e.g., a slow effect).

Implemented modifiers:
- Poison: Deals damage over time and makes character green.
- Freeze: Reduces the character's movement speed by 70% and makes character blue.

### Stats & Variables

StatTypes are implemented as ScriptableObjects to easily add new ones. Every character has CharacterStats responsible for storing stat values and applying stat modifiers. 

In WeaponActions, Variable objects are used. These are ScriptableObjects that can have a value of a float or a StatType. In this project,
I use a WeaponDamage Variable in all DealDamage actions for all three weapons, but each weapon can have its own Variable and its own StatType for damage as well. 

Expressions are not implemented but can be added as a third option in Variables to support any kind of multiple variable combinations.

### Leveling Up

When player levels up it recieves a Reward that may be a new random weapon or random upgrade.\
Upgrades are ScriptableObejcts storing stat modifiers and/or Requirements that unlock execution of different WeaponActions.


#### Optimizations
all characters, pickups and objects which are spawned by weapons are pooled.

Layer collision matrix only includes necesarry interactions, but enemies collide with each other and their local avoidance is left to Unitys physics to resolve 
and gets heavy when there are many enemies colliding with each other. It can be improved by using local avoidance of [A Star](https://arongranberg.com/astar/) or implementing a simple one.

#### Challenges Faced

The main challenge was that WeaponActions needed to be capable of interacting with any game system.

For instance, the CreateProjectile action required a WeaponObjectFactory which is responsible to spawn and pool any WeaponObject.
I had to decide whether to inject this or pass it as an argument to the Execute method.
Injecting into ScriptableObjects is tricky and not recommended, while passing it to all actions just for one to use felt inelegant. Additionally, future actions might require different objects, which would bloat the argument list.

The best solution I came up was to pass a single object containing all the different services that actions might need. However this is not a perfect solution as well.

