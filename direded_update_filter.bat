set "filter_link=https://raw.githubusercontent.com/direded/poe2-filter/refs/heads/master/filter"
set "documents_poe=C:\Users\%USERNAME%\Documents\My Games\Path of Exile 2"

if not exist "%documents_poe%\direded_filter" mkdir "%documents_poe%\direded_filter"
powershell -c "Invoke-WebRequest -Uri '%filter_link%/direded.filter' -OutFile '%documents_poe%\direded.filter'"
powershell -c "Invoke-WebRequest -Uri '%filter_link%/direded_filter/zombie_svo.mp3' -OutFile '%documents_poe%\direded_filter\zombie_svo.mp3'"