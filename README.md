# Informacje o aplikacji

Aplikacja wymaga uruchomionej lokalnej bazy danych **MS SQL Server**.

ConnectionString jest zdefiniowany w pliku:
```csharp
App.config
```

## Inicjalizacja bazy danych

Po uruchomieniu aplikacji:

- sprawdzane jest, czy baza danych oraz tabela istnieją,
- jeżeli nie istniejąm, zostają automatycznie utworzone,
- baza zostaje wypełniona przykładowymi danymi testowymi.

## Kontrolki DevExpress

Projekt wykorzystuje pakiet kontrolek **DevExpress** uruchamiany w wersji **trialowej**.

W projekcie znajduje się dodatkowy plik:
```csharp
DevExpress.XtraGrid.v25.2.resources.dll
```

Jest to plik odpowiedzialny za polską lokalizację kontrolki **Grid**.

Plik ten jest automatycznie kopiowany do odpowiedniego katalogu podczas procesu buildowania projektu.

