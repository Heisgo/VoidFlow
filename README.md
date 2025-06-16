# VoidFlow State Machine for Unity

Because your game's logic shouldn't feel like a tangled mess of `if…else` statements
A slick, plug‑and‑play finite‑state machine that lets you focus on fun behaviors, not boilerplate.

## What makes it rad?
* **Code‑driven & lightweight** -- No fancy node graphs required. Just inherit `FlowState` and you're good to go.
* **SOLID (OCP principle & SRP principle) & Extensible** -- Open for new behaviors (derive your own `FlowState`), closed for core hacks.
* **Built‑in timers** -- `Wait( seconds, callback )` right in your states, plus `TransitionAfter<T>` in the machine.
* **ScriptableObject kickoff** -- Drag a `ScriptableState` asset into your machine to pick your starting state.
* **Smart caching** -- States are lazy‑instanced and cached, so you don't burn CPU or memory with repeat `new` calls.

## Installing
1. Drop the `VoidFlow` folder into your `Assets/ThirdParty/Plugins/` (or anywhere under `Assets`);
2. (Optional) Create a `VoidFlowAssets` folder for your `ScriptableState` assets;
3. Add `using VoidFlow;` in your scripts;
4. You're set 🤙

## Quick Start Guide

### 1. Add the machine
* Select a GameObject (e.g. your enemy or NPC)
* Click **Add Component -> FlowMachine**

### 2. Create your states
* In `Scripts/States/`, make classes inheriting `FlowState`
* Override `OnEnter()`, `OnUpdate()`, `OnExit()` (there are 2 examples in VoidFlow/States so you can understand it better)

```csharp
public class EnemyIdleState : FlowState
{
    public override void OnEnter() => Debug.Log("Just chillin'");
    public override void OnUpdate()
    {
        if (Vector3.Distance(machine.transform.position, player.position) < 5f)
            machine.TransitionTo<EnemyChaseState>();
    }
}

public class EnemyChaseState : FlowState
{
    public override void OnEnter() => Debug.Log("Let's chase this mf!");
    public override void OnUpdate()
    {
        // chase logic…
        if (lostPlayer) machine.TransitionAfter<EnemyIdleState>(1.5f); // time helper (transition after 1.5 -- or any time u want -- seconds) functions 😉
    }
}
```

### 3. (Optional) Set start state
1. Right‑click Project window -> **Create -> VoidFlow -> Start State**
2. Name it (e.g. `StartState_Idle`)
3. In the Inspector, type your full class name
4. Drag this asset into the `FlowMachine` `startState` field

### 4. Hit Play
* Watch the automatic logs in Console (“\[Flow] Entering EnemyIdleState” etc.)
* See your FSM switch states seamlessly

## Full Example (you'll find this (but the player's script) in the plugin's files)
```csharp
// EnemyData.cs
[Serializable]
public class EnemyData {
    public float moveSpeed = 3f;
    public float viewDistance = 5f;
    public Transform idleTarget;
}

// PlayerController.cs
public class PlayerController : MonoBehaviour {
    public float speed = 5f;
    void Update() {
        var m = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
        transform.position += m * speed * Time.deltaTime;
    }
}

// EnemyIdleState.cs
public class EnemyIdleState : FlowState {
    float speed => machine.data.moveSpeed;
    Transform t => machine.transform;
    Transform player;

    public override void OnEnter() {
        player = GameObject.FindWithTag("Player")?.transform;
        Debug.Log("Just chilling...");
    }

    public override void OnUpdate() {
        if (player == null) return;
        // return to idleTarget if set...
        Vector3 target = machine.data.idleTarget ? machine.data.idleTarget.position : t.position;
        if (Vector3.Distance(t.position, target) > 0.1f)
            t.position = Vector3.MoveTowards(t.position, target, speed * Time.deltaTime);

        // detect player
        if (Vector3.Distance(t.position, player.position) < machine.data.viewDistance)
            machine.TransitionTo<EnemyChaseState>();
    }
}

// EnemyChaseState.cs
public class EnemyChaseState : FlowState {
    float speed => machine.data.moveSpeed;
    Transform t => machine.transform;
    Transform player;

    public override void OnEnter() {
        player = GameObject.FindWithTag("Player")?.transform;
        Debug.Log("Chasing this mf!");
    }

    public override void OnUpdate() {
        if (player == null) return;
        t.position = Vector3.MoveTowards(t.position, player.position, speed * Time.deltaTime);
        if (Vector3.Distance(t.position, player.position) > machine.data.viewDistance + 2f)
            machine.TransitionAfter<EnemyIdleState>(1f);
    }
}
```

## Customize It!
* **Use your own data holder**: Swap in a `ScriptableObject EnemyDataSO` for per‑type configs;
* **Extend FlowState**: Add helpers like `WaitUntil()`, `Repeat()`, or custom debug draws;
* **Visual debugging**: Override `OnDrawGizmos()` inside your states for per‑state gizmos;
* **Plug in VoidSignals**: Fire SO‑driven events on state changes for UI, sound FX, or analytics;
* And remember: Pull requests are always appreciated!

## Why not just raw `if`/`else`?
1. **Centralized control** -- All transitions in one place, no hidden branches;
2. **Reusability** -- Swap states between enemies, reuse chase logic everywhere;
3. **Testability** -- States are plain classes, unit‑test them without needing a scene;
4. **Clarity** -- No spaghetti code: each state only cares about its own job;
5. **Timers made easy** -- Built‑in `Wait` and `TransitionAfter` so you ditch extra coroutines;

## ❤️ Thanks for trying VoidFlow!
Built with caffeine, keyboard mashing, and a healthy dose of "please don't crash my code."
If it saved you from state‑machine headaches, drop a ⭐ on GitHub and share your cool setups ;D
