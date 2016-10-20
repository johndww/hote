using System;
using UnityEngine;

public class HeroUtil : MonoBehaviour
{
	private HeroUtil ()
	{
		//singleton
	}

	public static void SetHeroMarkerEnablement(Boolean value, GameObject character, String marker)
	{
		// pass true in to include inactive components. by default all char markers children entities are inactive
		var meshRenderers = character.GetComponentsInChildren<MeshRenderer>(true);

		foreach (MeshRenderer meshRenderer in meshRenderers) {
			if (meshRenderer.CompareTag(marker)) {
				// have to activate the gameObject holding the renderer component - not the renderer itself
				meshRenderer.gameObject.SetActive(value);
				return;
			}
		}
		throw new MissingComponentException("missing the " + marker + " component for " + character);
	}
}

