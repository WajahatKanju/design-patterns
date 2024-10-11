from abc import ABC, abstractmethod
from typing import Dict, List

class Investor(ABC):
    @abstractmethod
    def update(self, stock_name: str, price: float) -> None:
        """Notify the investor of a stock price update."""
        pass


class ConcreteInvestor(Investor):
    def __init__(self, name: str) -> None:
        self.name = name
  
    def update(self, stock_name: str, price: float) -> None:
        """Print a notification to the investor."""
        print(f"Investor {self.name} was notified. Price updated for STOCK: {stock_name}, to PRICE: ${price:.2f}")


class StockMarket(ABC):
    @abstractmethod
    def register_investor(self, investor: Investor) -> None:
        """Register a new investor."""
        pass

    @abstractmethod
    def remove_investor(self, investor: Investor) -> None:
        """Remove an investor."""
        pass

    @abstractmethod
    def notify_investors(self, stock_name: str) -> None:
        """Notify all investors of a stock price change."""
        pass

    @abstractmethod
    def set_stock_price(self, stock_name: str, price: float) -> None:
        """Set the stock price and notify investors."""
        pass
  
class ConcreteStockMarket(StockMarket):
    def __init__(self) -> None:
        self.investors: List[Investor] = []
        self.stock_prices:Dict[str, float] = {}

    def register_investor(self, investor: Investor) -> None:
        """Add an investor to the list."""
        self.investors.append(investor)

    def remove_investor(self, investor: Investor) -> None:
        """Remove an investor from the list."""
        self.investors.remove(investor)
  
    def notify_investors(self, stock_name: str) -> None:
        """Notify all investors of the price change for a specific stock."""
        for investor in self.investors:
            investor.update(stock_name, self.stock_prices[stock_name])
  
    def set_stock_price(self, stock_name: str, price: float) -> None:
        """Set the price of a stock and notify investors."""
        self.stock_prices[stock_name] = price
        self.notify_investors(stock_name)
