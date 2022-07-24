using System.Collections;
using UnityEngine;

namespace RPG.SceneManagement
{
	public class Fader : MonoBehaviour
	{
		CanvasGroup canvasGroup;

		private void Awake()
		{
			canvasGroup = GetComponent<CanvasGroup>();
		}

		IEnumerator FadeOutIn()
		{
			yield return FadeOut(3f);
			print("Faded Out");
			yield return FadeIn(1f);
			print("Faded In");
		}

		public void FadeOutImmediate()
		{
			canvasGroup.alpha = 1;
		}

		public IEnumerator FadeOut(float time)
		{
			while (canvasGroup.alpha < 1)
			{
				canvasGroup.alpha += Time.deltaTime / time;
				yield return null;
			}
		}

		public IEnumerator FadeIn(float time)
		{
			while (canvasGroup.alpha > 0)
			{
				canvasGroup.alpha -= Time.deltaTime / time;
				yield return null;
			}
		}
	}
}