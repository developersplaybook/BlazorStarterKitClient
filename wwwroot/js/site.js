window.loadScript = (src) => {
  return new Promise((resolve, reject) => {
    const script = document.createElement('script');
    script.src = src;
    script.onload = () => resolve();
    script.onerror = () => reject(new Error(`Failed to load script: ${src}`));
    document.head.appendChild(script);
  });
};

window.getDynamicStyle = (spriteType) => {
  const dayNumber = new Date().getDay() % 12; // Get day of the week and limit to 12
  const offset = -81 * dayNumber; // Calculate the offset
  return `background: url('/images/Bootstrap${spriteType}Composed.jpg') 0 ${offset}px transparent; height: 81px; margin: 0;`;
};


