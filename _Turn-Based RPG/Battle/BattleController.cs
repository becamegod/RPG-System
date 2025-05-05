using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using SkillSystem;

using UISystem;

using UnityEngine;

public class BattleController : Singleton<BattleController>
{
    [SerializeField] BattleCharacter[] charactersOnA;
    [SerializeField] BattleCharacter[] charactersOnB;

    [SerializeField] Menu actionMenu, skillMenu, targetMenu;
    [SerializeField] ButtonGeneratorByLabel targetGenerator, skillGenerator;
    [SerializeField] TextPopup textPopupPrefab;
    [SerializeField] TurnActionHandler turnActionHandler;
    [SerializeField] EffectResultHandler effectResultHandler;
    [SerializeField] bool log;

    // temp
    [SerializeField] SkillDefinition[] skills;
    public SkillDefinition[] Skills => skills;

    // fields
    List<BattleCharacter> turnQueue;
    Dictionary<BattleCharacter, IList<BattleCharacter>> allyMap, enemyMap;
    Queue<IEnumerator> sceneQueue;
    bool turnEnded;

    // props
    public Menu TargetMenu => targetMenu;
    public Menu SkillMenu => skillMenu;
    public Menu ActionMenu => actionMenu;
    public ButtonGeneratorByLabel TargetGenerator => targetGenerator;
    public ButtonGeneratorByLabel SkillGenerator => skillGenerator;

    // events
    public event Action<BattleCharacter, IEnumerable<BattleCharacter>> OnNewTurn, OnAction;
    public event Action OnTurnEnded;

    private new void Awake()
    {
        base.Awake();
        SetupSides();
        SetupTurns();
    }

    private void Start() => StartCoroutine(BattleCR());

    private void SetupSides()
    {
        allyMap = new();
        enemyMap = new();
        foreach (var character in charactersOnA)
        {
            allyMap[character] = charactersOnA;
            enemyMap[character] = charactersOnB;
        }
        foreach (var character in charactersOnB)
        {
            allyMap[character] = charactersOnB;
            enemyMap[character] = charactersOnA;
        }
    }

    private void SetupTurns()
    {
        turnQueue = new();
        foreach (var character in charactersOnA.Concat(charactersOnB)) turnQueue.Add(character);
    }

    private IEnumerator BattleCR()
    {
        sceneQueue = new();
        var i = 0;
        while (true)
        {
            var character = turnQueue[i];
            yield return StartCoroutine(TurnCR(character));
            i = (i + 1) % turnQueue.Count;
        }
    }

    private IEnumerator TurnCR(BattleCharacter character)
    {
        // start
        if (log) Debug.Log($"Start turn of: {character.gameObject.name}");
        turnEnded = false;

        // setup target names
        var enemies = enemyMap[character];
        var allies = allyMap[character];

        // event for idle camera
        OnNewTurn?.Invoke(character, enemies);

        // process turn start
        var results = character.BattleSubject.ProcessTurnStart();
        effectResultHandler.HandleResults(character, results);

        // do behavior
        sceneQueue.Enqueue(BehaviorCR());

        IEnumerator BehaviorCR()
        {
            // get turn action
            var behavior = character.GetComponent<BattleBehavior>();
            var behaviorWrapper = new BattleBehaviorWrapper(behavior);
            yield return behaviorWrapper.ExecuteCR(new()
            {
                allies = allies,
                enemies = enemies,
                character = character,
            });
            var action = behaviorWrapper.ConfirmedAction;

            // execute turn action
            turnActionHandler.ExecuteTurnAction(character, action);

            // event for action camera
            OnAction?.Invoke(character, action.targets);

            // wait for turn end
            yield return new WaitUntil(() => turnEnded);
        }

        // handle scene queue
        var sceneCR = sceneQueue.Dequeue();
        while (sceneCR != null)
        {
            yield return sceneCR;
            sceneCR = sceneQueue.Count > 0 ? sceneQueue.Dequeue() : null;
        }

        // process turn end
        results = character.BattleSubject.ProcessTurnEnd();
        effectResultHandler.HandleResults(character, results);

        // end
        character.LookAt(GetSideCenter(enemies));
        if (log) Debug.Log($"End turn of: {character.gameObject.name}");
        OnTurnEnded?.Invoke();
    }

    internal void EndTurn(string name = null)
    {
        if (log) Debug.Log($"Animator triggers end turn: {name ?? ""}");
        turnEnded = true;
    }

    internal IList<BattleCharacter> GetEnemies(BattleCharacter battleCharacter) => enemyMap[battleCharacter];

    public static Vector3 GetSideCenter(IList<BattleCharacter> characters)
    {
        Vector3 center = characters.Aggregate(Vector3.zero, (stack, enemy) => stack + enemy.transform.position);
        center /= characters.Count;
        return center;
    }
}