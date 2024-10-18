window.apiHelper = {
  getHelper: async (url) => {
    const response = await fetch(url, {
      headers: {
        "Content-Type": "application/json",
        Accept: "application/json",
        "Accept-Language": "*"
      },
      method: "GET",
      credentials: 'include',
    });

    if (!response.ok) throw new Error(`Request failed with status ${response.status}`);

    return response.json();
  }
}
