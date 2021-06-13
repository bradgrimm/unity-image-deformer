using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ScenePreparation))]
public class AblationManager : MonoBehaviour
{
    public bool allButOne = true;
    public int shotsPerRandomizer = 2;
    public int waitFramesForSceneToSettle = 10;
    IEnumerator randomizerEnumerator;
    ScenePreparation scenePreparation;

    private bool isRunning = false;
    private bool isSingleShot = false;
    private int currentPhotoCount = 0;
    private int totalPhotoCount = 0;
    private int waitingFrames = 0;
    private State state = State.SetupRandomizers;
    private string subDirName = "";

    private Randomizer[] randomizers = new Randomizer[0];

    enum State {
        SetupRandomizers,
        SetupScene,
        Save,
        Complete,
    }

    void Start()
    {
        Stop();

        if (!Application.isEditor)
            Run();
        
        scenePreparation = GetComponent<ScenePreparation>();
    }

    Randomizer[] FindRandomizers()
    {
        if (randomizers.Length == 0)
            randomizers = Resources.FindObjectsOfTypeAll<Randomizer>();
        return randomizers;
    }

    IEnumerator SingleRandomizer()
    {
        Randomizer[] randomizers = FindRandomizers();
        foreach (Randomizer randomizer in randomizers)
        {
            if (randomizer.includeInAblations)
                randomizer.triggerEnabled = false;
        }
        
        foreach (Randomizer randomizer in randomizers)
        {
            if (randomizer.includeInAblations)
            {
                string name = randomizer.GetType().Name;
                subDirName = name.Replace("Randomizer", "");
                Debug.Log("Enabling ablation: " + name);
                float prevChange = randomizer.triggerChance;
                randomizer.triggerChance = 1.0f;
                randomizer.triggerEnabled = true;
                yield return 0;
                randomizer.triggerChance = prevChange;
                randomizer.triggerEnabled = false;
            }
        }
    }

    IEnumerator AllButOneRandomizer()
    {
        Randomizer[] randomizers = FindRandomizers();
        foreach (Randomizer randomizer in randomizers)
        {
            if (randomizer.includeInAblations)
                randomizer.triggerEnabled = true;
        }
        
        foreach (Randomizer randomizer in randomizers)
        {
            if (randomizer.includeInAblations)
            {
                string name = randomizer.GetType().Name;
                subDirName = name.Replace("Randomizer", "");
                Debug.Log("Removing ablation: " + name);
                float prevChange = randomizer.triggerChance;
                randomizer.triggerEnabled = false;
                yield return 0;
                randomizer.triggerEnabled = true;
            }
        }
    }

    void Update()
    {
        if (!isRunning && !isSingleShot)
            return;

        if (waitingFrames > 0)
        {
            waitingFrames--;
            return;
        }

        switch (state)
        {
            case State.SetupRandomizers:
                state = randomizerEnumerator.MoveNext() ? State.SetupScene : State.Complete;
                currentPhotoCount = 0;
                break;
            case State.SetupScene:
                scenePreparation.GenerateScene();
                waitingFrames = waitFramesForSceneToSettle;
                state = State.Save;
                break;
            case State.Save:
                scenePreparation.SaveSceneToDisk(subDirName);
                currentPhotoCount++;
                totalPhotoCount++;
                state = currentPhotoCount >= shotsPerRandomizer ? State.SetupRandomizers : State.SetupScene;
                isSingleShot = false;
                break;
        }
    }

    public void Step()
    {
        isSingleShot = true;
    }

    public void Run()
    {
        Stop();
        isRunning = true;
    }

    public void Stop()
    {
        isRunning = false;
        totalPhotoCount = currentPhotoCount = waitingFrames = 0;
        randomizerEnumerator = allButOne ? AllButOneRandomizer() : SingleRandomizer();
        state = State.SetupRandomizers;
    }
}
