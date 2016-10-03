using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIControls : MonoBehaviour {
    // The following are the images that make the hero buttons look selected or light up
    public Image HeroOneSelected;
    public Image HeroTwoSelected;
    public Image HeroThreeSelected;
    public Image HeroFourSelected;

    // We can use the following to determine which heroe slots we have selected.
    bool HeroOneSlot = false;
    bool HeroTwoSlot = false;
    bool HeroThreeSlot = false;
    bool HeroFourSlot = false;

    // We'll use the following as the variables passed from the hero selection screen
    string HeroSlotOne = "Hero1";
    string HeroSlotTwo = "Hero2";
    string HeroSlotThree = "Hero5";
    string HeroSlotFour = "Hero6";

    // We'll attach the 6 different ability UIs for the 6 heroes bellow. Each hero will have different abilities with different icons so as a result they need to be created separately.
    public GameObject HeroOneAbilities;
    public GameObject HeroTwoAbilities;
    public GameObject HeroThreeAbilities;
    public GameObject HeroFourAbilities;
    public GameObject HeroFiveAbilities;
    public GameObject HeroSixAbilities;

    // Only 4 ability UIs can be active, one for each hero, so we'll add the slots bellow to pair them up
    public GameObject SlotOneAbilities;
    public GameObject SlotTwoAbilities;
    public GameObject SlotThreeAbilities;
    public GameObject SlotFourAbilities;

    // This is used to select multiple heroes. When it's set to true, clicking on another button won't turn the other ones off.
    bool HeroClicked = false;

    void Start()
    {
        // Based on the variables we got from the selection menu, we'll attach the correct ability UIs to each of the 4 slots
        if (HeroSlotOne == "Hero1") { SlotOneAbilities = HeroOneAbilities; }
        else if (HeroSlotOne == "Hero2") { SlotOneAbilities = HeroTwoAbilities; }
        else if (HeroSlotOne == "Hero3") { SlotOneAbilities = HeroThreeAbilities; }
        else if (HeroSlotOne == "Hero4") { SlotOneAbilities = HeroFourAbilities; }
        else if (HeroSlotOne == "Hero5") { SlotOneAbilities = HeroFiveAbilities; }
        else if (HeroSlotOne == "Hero6") { SlotOneAbilities = HeroSixAbilities; }

        if (HeroSlotTwo == "Hero1") { SlotTwoAbilities = HeroOneAbilities; }
        else if (HeroSlotTwo == "Hero2") { SlotTwoAbilities = HeroTwoAbilities; }
        else if (HeroSlotTwo == "Hero3") { SlotTwoAbilities = HeroThreeAbilities; }
        else if (HeroSlotTwo == "Hero4") { SlotTwoAbilities = HeroFourAbilities; }
        else if (HeroSlotTwo == "Hero5") { SlotTwoAbilities = HeroFiveAbilities; }
        else if (HeroSlotTwo == "Hero6") { SlotTwoAbilities = HeroSixAbilities; }

        if (HeroSlotThree == "Hero1") { SlotThreeAbilities = HeroOneAbilities; }
        else if (HeroSlotThree == "Hero2") { SlotThreeAbilities = HeroTwoAbilities; }
        else if (HeroSlotThree == "Hero3") { SlotThreeAbilities = HeroThreeAbilities; }
        else if (HeroSlotThree == "Hero4") { SlotThreeAbilities = HeroFourAbilities; }
        else if (HeroSlotThree == "Hero5") { SlotThreeAbilities = HeroFiveAbilities; }
        else if (HeroSlotThree == "Hero6") { SlotThreeAbilities = HeroSixAbilities; }

        if (HeroSlotFour == "Hero1") { SlotFourAbilities = HeroOneAbilities; }
        else if (HeroSlotFour == "Hero2") { SlotFourAbilities = HeroTwoAbilities; }
        else if (HeroSlotFour == "Hero3") { SlotFourAbilities = HeroThreeAbilities; }
        else if (HeroSlotFour == "Hero4") { SlotFourAbilities = HeroFourAbilities; }
        else if (HeroSlotFour == "Hero5") { SlotFourAbilities = HeroFiveAbilities; }
        else if (HeroSlotFour == "Hero6") { SlotFourAbilities = HeroSixAbilities; }

        // We'll activate the first hero and the first slot UI for test purposes
        HeroOneButtonActions();
        SlotOneAbilities.gameObject.SetActive(true);
    }


    // Once a hero button is pressed, if you press another hero button within 0.2 seconds, they both light up. This timer resets each time a new button is pressed. This way you can select multiple buttons in a quick swipe over their potraits.
    IEnumerator MultipleHeroes() {
        yield return new WaitForSeconds(0.2F);
        HeroClicked = false;
    }



    public void HeroOneButtonActions() //Hero One button is clicked
    {
        StopCoroutine(MultipleHeroes()); // Stops the timer that will turn HeroClicked to false and restarts it at the end of this function.
        HeroOneSelected.gameObject.SetActive(true); // Makes this button appear selected or light up.
        HeroOneSlot = true; // Stating that a hero has been selected
        SlotOneAbilities.gameObject.SetActive(true); // Activate the UI with the correct skills for this hero

        SlotTwoAbilities.gameObject.SetActive(false); // Hide the other ability UIs so only the correct one is visible
        SlotThreeAbilities.gameObject.SetActive(false); // Hide the other ability UIs so only the correct one is visible
        SlotFourAbilities.gameObject.SetActive(false); // Hide the other ability UIs so only the correct one is visible

        if (!HeroClicked) // If no other button was clicked recently, it means we're not in the process of selecting multiple heroes so we're turning the othher ones off.
        {
            HeroTwoSelected.gameObject.SetActive(false);  // Makes this button appear unselected.
            HeroThreeSelected.gameObject.SetActive(false);  // Makes this button appear unselected.
            HeroFourSelected.gameObject.SetActive(false);  // Makes this button appear unselected.

            HeroTwoSlot = false; // Deselecting the rest of the heroes
            HeroThreeSlot = false; // Deselecting the rest of the heroes
            HeroFourSlot = false; // Deselecting the rest of the heroes
        }

        HeroClicked = true; // We're setting this to true briefly to allow us to select multiple heroes if we want to.
        StartCoroutine(MultipleHeroes()); // We'll set HeroClicked to false in 0.2 seconds so that afterwards when we select a hero the other ones turn off.
    }



    public void HeroTwoButtonActions() //Hero Two button is clicked
    {
        StopCoroutine(MultipleHeroes()); // Stops the timer that will turn HeroClicked to false and restarts it at the end of this function.
        HeroTwoSelected.gameObject.SetActive(true); // Makes this button appear selected or light up.
        HeroTwoSlot = true; // Stating that a hero has been selected
        SlotTwoAbilities.gameObject.SetActive(true); // Activate the UI with the correct skills for this hero

        SlotOneAbilities.gameObject.SetActive(false); // Hide the other ability UIs so only the correct one is visible
        SlotThreeAbilities.gameObject.SetActive(false); // Hide the other ability UIs so only the correct one is visible
        SlotFourAbilities.gameObject.SetActive(false); // Hide the other ability UIs so only the correct one is visible

        if (!HeroClicked) // If no other button was clicked recently, it means we're not in the process of selecting multiple heroes so we're turning the othher ones off.
        {
            HeroOneSelected.gameObject.SetActive(false);  // Makes this button appear unselected.
            HeroThreeSelected.gameObject.SetActive(false);  // Makes this button appear unselected.
            HeroFourSelected.gameObject.SetActive(false);  // Makes this button appear unselected.

            HeroOneSlot = false; // Deselecting the rest of the heroes
            HeroThreeSlot = false; // Deselecting the rest of the heroes
            HeroFourSlot = false; // Deselecting the rest of the heroes
        }

        HeroClicked = true; // We're setting this to true briefly to allow us to select multiple heroes if we want to.
        StartCoroutine(MultipleHeroes()); // We'll set HeroClicked to false in 0.2 seconds so that afterwards when we select a hero the other ones turn off.
    }



    public void HeroThreeButtonActions() //Hero Three button is clicked
    {
        StopCoroutine(MultipleHeroes()); // Stops the timer that will turn HeroClicked to false and restarts it at the end of this function.
        HeroThreeSelected.gameObject.SetActive(true); // Makes this button appear selected or light up.
        HeroThreeSlot = true; // Stating that a hero has been selected
        SlotThreeAbilities.gameObject.SetActive(true); // Activate the UI with the correct skills for this hero

        SlotOneAbilities.gameObject.SetActive(false); // Hide the other ability UIs so only the correct one is visible
        SlotTwoAbilities.gameObject.SetActive(false); // Hide the other ability UIs so only the correct one is visible
        SlotFourAbilities.gameObject.SetActive(false); // Hide the other ability UIs so only the correct one is visible

        if (!HeroClicked) // If no other button was clicked recently, it means we're not in the process of selecting multiple heroes so we're turning the othher ones off.
        {
            HeroOneSelected.gameObject.SetActive(false);  // Makes this button appear unselected.
            HeroTwoSelected.gameObject.SetActive(false);  // Makes this button appear unselected.
            HeroFourSelected.gameObject.SetActive(false);  // Makes this button appear unselected.

            HeroOneSlot = false; // Deselecting the rest of the heroes
            HeroTwoSlot = false; // Deselecting the rest of the heroes
            HeroFourSlot = false; // Deselecting the rest of the heroes
        }

        HeroClicked = true; // We're setting this to true briefly to allow us to select multiple heroes if we want to.
        StartCoroutine(MultipleHeroes()); // We'll set HeroClicked to false in 0.2 seconds so that afterwards when we select a hero the other ones turn off.
    }



    public void HeroFourButtonActions() //Hero Four button is clicked
    {
        StopCoroutine(MultipleHeroes()); // Stops the timer that will turn HeroClicked to false and restarts it at the end of this function.
        HeroFourSelected.gameObject.SetActive(true); // Makes this button appear selected or light up.
        HeroFourSlot = true; // Stating that a hero has been selected
        SlotFourAbilities.gameObject.SetActive(true); // Activate the UI with the correct skills for this hero

        SlotOneAbilities.gameObject.SetActive(false); // Hide the other ability UIs so only the correct one is visible
        SlotTwoAbilities.gameObject.SetActive(false); // Hide the other ability UIs so only the correct one is visible
        SlotThreeAbilities.gameObject.SetActive(false); // Hide the other ability UIs so only the correct one is visible

        if (!HeroClicked) // If no other button was clicked recently, it means we're not in the process of selecting multiple heroes so we're turning the othher ones off.
        {
            HeroOneSelected.gameObject.SetActive(false);  // Makes this button appear unselected.
            HeroTwoSelected.gameObject.SetActive(false);  // Makes this button appear unselected.
            HeroThreeSelected.gameObject.SetActive(false);  // Makes this button appear unselected.

            HeroOneSlot = false; // Deselecting the rest of the heroes
            HeroTwoSlot = false; // Deselecting the rest of the heroes
            HeroThreeSlot = false; // Deselecting the rest of the heroes
        }

        HeroClicked = true; // We're setting this to true briefly to allow us to select multiple heroes if we want to.
        StartCoroutine(MultipleHeroes()); // We'll set HeroClicked to false in 0.2 seconds so that afterwards when we select a hero the other ones turn off.
    }

}
