from abc import ABC, abstractmethod
from typing import List

class DisplayObserver(ABC):
    @abstractmethod
    def update(self, temperature: float, humidity: float, pressure: float) -> None:
        pass

class DisplayElement(ABC):
    @abstractmethod
    def display(self) -> None:
        pass


class ConcreteDisplay(DisplayElement, DisplayObserver):
    def __init__(self, name: str, temperature: float = 0, humidity: float = 0, pressure: float = 0) -> None:
        self.name = name
        self.temperature: float = temperature
        self.humidity: float = humidity
        self.pressure: float = pressure

    def update(self, temperature: float, humidity: float, pressure: float) -> None:
        self.temperature = temperature
        self.humidity = humidity
        self.pressure = pressure
        self.display()

    def display(self) -> None:
        print(f"Current Weather Status[{self.name}]:")
        print("Temperature (Celsius):", self.temperature)
        print("Humidity:", self.humidity)
        print("Pressure:", self.pressure)
    

class SubjectWeatherStation(ABC):
    @abstractmethod
    def register_observer(self, o: DisplayObserver) -> None:
        pass

    @abstractmethod
    def remove_observer(self, o: DisplayObserver) -> None:
        pass

    @abstractmethod
    def notify_observers(self) -> None:
        pass

class ConcreteWeatherStation(SubjectWeatherStation):
    def __init__(self, temperature: float = 0, humidity: float = 0, pressure: float = 0) -> None:
        self.temperature: float = temperature
        self.humidity: float = humidity
        self.pressure: float = pressure
        self.observers: List[DisplayObserver] = []

    def register_observer(self, o: DisplayObserver) -> None:
        self.observers.append(o)
    
    def remove_observer(self, o: DisplayObserver) -> None:
        self.observers.remove(o)
    
    def notify_observers(self) -> None:
        for observer in self.observers:
            observer.update(self.temperature, self.humidity, self.pressure)
    
    def set_measurements_changed(self, temperature: float, humidity: float, pressure: float) -> None:
        self.temperature = temperature
        self.humidity = humidity
        self.pressure = pressure
        self.notify_observers()

if __name__ == "__main__":
    weather_station = ConcreteWeatherStation()

    display1 = ConcreteDisplay("Analog")
    display2 = ConcreteDisplay("Digital")

    weather_station.register_observer(display1)
    weather_station.register_observer(display2)

    weather_station.set_measurements_changed(25.0, 65.0, 1013.0)
    weather_station.set_measurements_changed(28.0, 70.0, 1012.5)