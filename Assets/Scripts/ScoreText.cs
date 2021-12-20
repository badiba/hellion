using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreText
{
    // TODO: Create a generic animation system.

    private const float _animationStageDuration = 0.10f;
    private const int _animationStageEndFontSize = 100;
    private readonly int _initialFontSize;
    private readonly Vector3 _initialPosition;
    private readonly Vector3 _animationStageEndPosition;
    private readonly Text _text;

    private float _animationProgress;
    private bool _isInAnimation = false;

    public ScoreText(Text text)
    {
        _text = text;
        _initialFontSize = _text.fontSize;

        _initialPosition = _text.transform.position;
        _animationStageEndPosition = _initialPosition + new Vector3(6, -6, 0);
    }

    public void Update()
    {
        if (_isInAnimation)
        {
            _animationProgress += Time.deltaTime / _animationStageDuration;

            if (_animationProgress < 0.5f)
            {
                var fontIncreaseProgress = _animationProgress * 2;
                var additionalFontSize = (int)((_animationStageEndFontSize - _initialFontSize) * fontIncreaseProgress);
                _text.fontSize = additionalFontSize + _initialFontSize;
                _text.transform.position = Vector3.Lerp(_initialPosition, _animationStageEndPosition, fontIncreaseProgress);
            }
            else if (_animationProgress < 1f)
            {
                var fontDecreaseProgress = (_animationProgress - 0.5f) * 2;
                var additionalFontSize = (int)((_animationStageEndFontSize - _initialFontSize) * (1 - fontDecreaseProgress));
                _text.fontSize = additionalFontSize + _initialFontSize;
                _text.transform.position = Vector3.Lerp(_animationStageEndPosition, _initialPosition, fontDecreaseProgress);
            }
            else if (_animationProgress >= 1f)
            {
                EndAnimation();
            }
            else
            {
                Debug.Assert(false, $"value of _animationProgress is not in an expected range: {_animationProgress}");
            }
        }
    }

    public void UpdateText(string text)
    {
        _text.text = text;
    }

    public void StartAnimation()
    {
        _animationProgress = 0f;
        _isInAnimation = true;
    }

    private void EndAnimation()
    {
        _animationProgress = 0f;
        _text.fontSize = _initialFontSize;
        _text.transform.position = _initialPosition;
        _isInAnimation = false;
    }
}
