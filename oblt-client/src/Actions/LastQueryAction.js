export const loadInitialData = async (
    fetchBusLocations,
    setDepartureDate,
    setValue1,
    setInputValue1,
    setSuggestions1,
    setValue2,
    setInputValue2,
    setSuggestions2,
    tomorrow
  ) => {
    try {
      const data = await fetchBusLocations(null);
      if (data.length > 0) {
        setSuggestions1(data);
        setSuggestions2(data);
  
        const savedOrigin = localStorage.getItem('lastOrigin');
        const savedDestination = localStorage.getItem('lastDestination');
        const savedDepartureDate = localStorage.getItem('lastDepartureDate');
  
        let parsedOrigin = null;
        if (savedOrigin) {
          parsedOrigin = JSON.parse(savedOrigin);
        }
  
        let parsedDestination = null;
        if (savedDestination) {
          parsedDestination = JSON.parse(savedDestination);
        }
  
        const origin = data.find((loc) => loc.id === parsedOrigin?.id);
        const destination = data.find((loc) => loc.id === parsedDestination?.id);
  
        if (origin) {
          setValue1(origin);
          setInputValue1(origin.name);
        } else if (data.length > 1) {
          setValue1(data[1]);
          setInputValue1(data[1]?.name || '');
        } else {
          setValue1(data[0]);
          setInputValue1(data[0]?.name || '');
        }
  
        if (destination) {
          setValue2(destination);
          setInputValue2(destination.name);
        } else if (data.length > 1) {
          setValue2(data[1]);
          setInputValue2(data[1]?.name || '');
        } else {
          setValue2(data[0]);
          setInputValue2(data[0]?.name || '');
        }
  
        if (savedDepartureDate) {
          const savedDate = new Date(savedDepartureDate);
          const todayDate = new Date();
          todayDate.setHours(0, 0, 0, 0); // Normalize to start of day
  
          if (savedDate >= todayDate) {
            setDepartureDate(savedDepartureDate);
          } else {
            setDepartureDate(tomorrow);
            localStorage.setItem('lastDepartureDate', tomorrow); // Update to tomorrow
          }
        } else {
          setDepartureDate(tomorrow);
        }
      }
    } catch (error) {
      console.error('Error loading initial data:', error);
    }
  };
  