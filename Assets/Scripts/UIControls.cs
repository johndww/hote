using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public class UIControls : MonoBehaviour {

    // player that these UI controls affect
    public Player player;

    // button lightups
    public Image[] imageBySlot = new Image[4];

    // all character ui abilities
    public GameObject earthMageAbilityUi;
    // TODO add the rest of the characters

    private Dictionary<HeroType, GameObject> heroTypeToAbilitiesUi;

    void Start()
    {
        this.heroTypeToAbilitiesUi = generateHeroTypeToUIMapping();
    }

    // ui abilities - derived from character selection
    private GameObject[] abilitiesBySlot = new GameObject[4];
    private Hero[] heroBySlot = new Hero[4];

    // current selections
    private GameObject currentAbilities;
    private Image currentImage;
    private Hero currentHero;

    // helper enum to map the slot to an array index
    enum HeroSlot { ONE = 0, TWO = 1, THREE = 2, FOUR = 3 }

    /// <summary>
    /// 
    /// Main UI selection entry point for selecting a new hero to control.
    /// 
    /// </summary>
    /// <param name="slotName">ONE,TWO,THREE,FOUR</param>
    public void selectHeroSlot(string slotName)
    {
        // check if the game is initialized yet. we may not have
        if (this.currentImage == null || this.currentAbilities == null)
        {
            initialize();
        }

        HeroSlot selectedSlot = getSlotByName(slotName);
        
        // deactivate previous selection
        this.currentImage.gameObject.SetActive(false);
        this.currentAbilities.gameObject.SetActive(false);

        // update new selections
        this.currentImage = this.imageBySlot[(int)selectedSlot];
        this.currentAbilities = this.abilitiesBySlot[(int)selectedSlot];
        this.currentHero = this.heroBySlot[(int)selectedSlot];

        // activate new selection
        this.currentImage.gameObject.SetActive(true);
        this.currentAbilities.gameObject.SetActive(true);
        this.currentHero.Selected();

        // update the selected hero for the Player
        this.player.SelectedCharacter = this.currentHero.gameObject;
    }

    /// <summary>
    /// 
    /// Main entry point for UI attack button selection
    /// 
    /// </summary>
    /// <param name="attackColor">GREEN,BLUE,RED,PURPLE</param>
    public void attack(string attackColor)
    {
        AttackType type = getAttack(attackColor);
        this.currentHero.Attack(type);
    }

    private void initialize()
    {
        GameObject[] characters = player.Characters;

        for (int i = 0; i < 4; i++) {
            Hero hero = characters[i].GetComponent<Hero>();
            GameObject uiAbilitiesForHeroSlot = this.heroTypeToAbilitiesUi[hero.GetHeroType()];

            this.abilitiesBySlot[i] = uiAbilitiesForHeroSlot;
            this.heroBySlot[i] = hero;
        }
        this.currentAbilities = this.abilitiesBySlot[0];
        this.currentImage = this.imageBySlot[0];
        this.currentHero = this.heroBySlot[0];
    }

    private HeroSlot getSlotByName(string slotName)
    {
        switch (slotName)
        {
            case "ONE":
                return HeroSlot.ONE;
            case "TWO":
                return HeroSlot.TWO;
            case "THREE":
                return HeroSlot.THREE;
            case "FOUR":
                return HeroSlot.FOUR;
            default:
                throw new ArgumentException("unexpected slotname: " + slotName);
        }
    }

    AttackType getAttack(string color)
    {
        switch (color)
        {
            case "GREEN":
                return AttackType.GREEN;
            case "BLUE":
                return AttackType.BLUE;
            case "RED":
                return AttackType.RED;
            case "PURPLE":
                return AttackType.PURPLE;
            default:
                throw new System.ArgumentException("unexpected color: " + color);
        }
    }

    private Dictionary<HeroType, GameObject> generateHeroTypeToUIMapping()
    {
        Dictionary<HeroType, GameObject> map = new Dictionary<HeroType, GameObject>();
        map.Add(HeroType.EarthMage, this.earthMageAbilityUi);
        //TODO add other abilities by type
        return map;
    }
}
