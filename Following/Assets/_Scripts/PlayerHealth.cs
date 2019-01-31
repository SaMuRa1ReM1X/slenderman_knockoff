using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour 
{
    public float _health = 100.0f;
    float _startingHealth;
    public float _healthDecayRate = 5.0f;
    float _decayModifier = 0.0f;

    public Renderer _staticRenderer;

	// Use this for initialization
	void Start () 
    {
        _startingHealth = _health;

        _decayModifier = _startingHealth / _healthDecayRate;

        

        if (!_staticRenderer)
        {
            GameObject staticObject = GameObject.Find("StaticObject");

            if (staticObject)
            {
                _staticRenderer = staticObject.renderer;
            }
            else
            {
                Debug.Log("No StaticObject not found!");
            }
        }

        Color alphaColor = _staticRenderer.material.color;
        alphaColor.a = 0.0f;
        _staticRenderer.material.color = alphaColor;
	}
    
    public void DecreaseHealth()
    {
        _health -= _decayModifier * Time.deltaTime;

        //Calculate Alpha of the Static Screen!
        float newAlpha = 1.0f - (_health / _startingHealth);

        Color alphaColor = _staticRenderer.material.color;
        alphaColor.a = newAlpha;
        _staticRenderer.material.color = alphaColor;

        //check if health is below 0! == DEAD
        if (_health <= 0.0f)
        {
            _health = 0.0f;

            Debug.Log("Player is out of health!");
            //Lose Condition

            //Load Game Over Scene!
            Application.LoadLevel("GameLose");
        }

        OffsetMainTexture();
    }

    public void IncreaseHealth()
    {
        _health += _decayModifier * Time.deltaTime;

        //Calculate Alpha of the Static Screen!
        float newAlpha = 1.0f - (_health / _startingHealth);

        Color alphaColor = _staticRenderer.material.color;
        alphaColor.a = newAlpha;
        _staticRenderer.material.color = alphaColor;

        //check if health is Above maximum health....
        if (_health >= _startingHealth)
        {
            _health = _startingHealth;
        }

        OffsetMainTexture();
    }

    public void OffsetMainTexture()
    {
        float randXOffset = Random.value;
        float randYOffset = Random.value;

        _staticRenderer.material.mainTextureOffset = new Vector2(randXOffset, randYOffset);
    }

}
