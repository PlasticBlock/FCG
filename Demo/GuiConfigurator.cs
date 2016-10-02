// Licensed under GPLv3 license or under special license
// See the LICENSE file in the project root for more information
// -----------------------------------------------------------------------
// Author: Plastic Block <admin@plasticblock.xyz>
// skype: plasticblock, email: support@plasticblock.xyz
// project: Fractal Caves Generator (FCG)
// -----------------------------------------------------------------------
#if DEBUG

using System.Collections;
using UnityEngine;

namespace FCG.Misc
{
	public class GuiConfigurator : MonoBehaviour
	{
		public EnvironmentGenerator environment;

		private bool _animateGeneration;
		private int _animationSpeed;
		
		private void OnGUI()
		{
			GUI.backgroundColor = Color.blue + Color.white / 2;
			GUI.contentColor = Color.yellow;

			GUILayout.BeginArea(new Rect(0, 0, Screen.width / 5, Screen.height / 2));

			if (_animateGeneration)
			{
				environment.generateRandomSeed = false;
				if (GUILayout.Button("Stop"))
				{
					StopAllCoroutines();
					_animateGeneration = false;
				}
				GUILayout.Label("Speed");
				_animationSpeed = (int)GUILayout.HorizontalScrollbar(_animationSpeed, 1f, 1f, 100f);
				GUILayout.EndArea();
				return;
			}

			if (GUILayout.Button(string.Format("New Seed. Current: {0}", environment.seed)))
				environment.seed = Random.Range(1000000, 9999999);

			environment.generateRandomSeed = GUILayout.Toggle(environment.generateRandomSeed, "Auto Random Seed");

			GUILayout.Label("Border");
			environment.border = (int)GUILayout.HorizontalScrollbar(environment.border, 1f, 2f, (float) environment.size.y/2);

			GUILayout.Label("Cave Length");
			environment.caveLength = (int)GUILayout.HorizontalScrollbar(environment.caveLength, 1f, 2f, (float)(environment.size.y - environment.border) * (environment.size.x - environment.border) / 2);

			if (GUILayout.Button("Generate"))
				environment.Generate();

			if (GUILayout.Button("Start Animated Generation"))
			{
				_animateGeneration = true;
				environment.caveLength = 0;
				StartCoroutine(AnimateGeneration());
			}

			GUILayout.EndArea();
		}

		private IEnumerator AnimateGeneration()
		{
			yield return new WaitForEndOfFrame();
			if (environment.caveLength >= (environment.size.y - environment.border)*(environment.size.x - environment.border)/2)
			{
				_animateGeneration = false;
				yield break;
			}

			environment.caveLength += _animationSpeed;
			environment.Generate();
			StartCoroutine(AnimateGeneration());
		}
	}
}

#endif