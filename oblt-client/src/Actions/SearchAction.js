export const handleSearch = (
    value1,
    value2,
    departureDate,
    navigate
  ) => {
    if (!value1 || !value2) {
      alert('Please select both origin and destination locations.');
      return;
    }
  
    // Save the current values to localStorage
    localStorage.setItem('lastOrigin', JSON.stringify(value1));
    localStorage.setItem('lastDestination', JSON.stringify(value2));
    localStorage.setItem('lastDepartureDate', departureDate);
  
    // Construct the URL
    const originId = value1.id;
    const destinationId = value2.id;
    const dynamicPath = `${originId}-${destinationId}`;
  
    navigate(`/seferler/${dynamicPath}/${departureDate}`, {
      state: {
        origin: value1,
        destination: value2,
        departureDate: departureDate,
      },
    });
  };
  