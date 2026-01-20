# KatalogKlientow

Aplikacja wymaga uruchomionego MS SQL local DB.
ConnectionString zdefiniowany jest w pliku App.config
Po uruchomieniu aplikacji, sprawdzane jest czy baza i tabela istnieje. Jeżeli nie istnieje to zostanie ona stworzona, oraz wypełniona testowymi danymi.

Projekt wykorzystuje pakiet kontrolek DevExpress, uruchamiane na wersji trialowej.
W projekcie znajduje sie dodatkowy plik "DevExpress.XtraGrid.v25.2.resources.dll" jest to translacja kontroli Grid na język polski, zostanie ona skopiowana w odpowiednie miejsce w trakcie buildowania projektu,
