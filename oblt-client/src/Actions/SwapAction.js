export const handleSwap = (
    value1,
    setValue1,
    inputValue1,
    setInputValue1,
    suggestions1,
    setSuggestions1,
    value2,
    setValue2,
    inputValue2,
    setInputValue2,
    suggestions2,
    setSuggestions2
  ) => {
    const tempValue = value1;
    const tempInputValue = inputValue1;
    const tempSuggestions = suggestions1;

    setValue1(value2);
    setInputValue1(inputValue2);
    setSuggestions1(suggestions2);

    setValue2(tempValue);
    setInputValue2(tempInputValue);
    setSuggestions2(tempSuggestions);
  };
  