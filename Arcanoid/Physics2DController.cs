using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Arcanoid
{
    class Physics2DController
    {
        // Константы для размеров клеток и значений карты
        private const int CellSize = 20;
        private const int MaxCellValue = 99;
        private const int MinBrickValue = 10;

        public bool IsCollide(Player player, MapController map, Label scoreLabel)
        {
            bool isColliding = false;

            // Проверка столкновения с боковыми границами карты
            if (player.ballX / CellSize + player.dirX > MapController.mapWidth - 1 || player.ballX / CellSize + player.dirX < 0)
            {
                player.dirX *= -1; // Обратное направление движения по оси X
                isColliding = true;
            }

            // Проверка столкновения с верхней границей карты
            if (player.ballY / CellSize + player.dirY < 0)
            {
                player.dirY *= -1; // Обратное направление движения по оси Y
                isColliding = true;
            }

            // Получаем индексы ячейки на карте, в которой находится мяч
            int cellX = player.ballX / CellSize;
            int cellY = player.ballY / CellSize;

            // Проверка столкновения с блоком на карте
            if (map.map[cellY + player.dirY, cellX] != 0)
            {
                isColliding = true;

                // Обработка разрушения блока
                if (map.map[cellY + player.dirY, cellX] > MinBrickValue && map.map[cellY + player.dirY, cellX] < MaxCellValue)
                {
                    map.map[cellY + player.dirY, cellX] = 0; // Уничтожаем блок
                    map.map[cellY + player.dirY, cellX - 1] = 0; // Уничтожаем блок слева
                    player.score += 50; // Увеличиваем счет
                    if (player.score % 200 == 0 && player.score > 0)
                    {
                        map.AddLine(); // Добавляем новую строку блоков
                    }
                }
                else if (map.map[cellY + player.dirY, cellX] < MinBrickValue)
                {
                    map.map[cellY + player.dirY, cellX] = 0; // Уничтожаем блок
                    map.map[cellY + player.dirY, cellX + 1] = 0; // Уничтожаем блок справа
                    player.score += 50; // Увеличиваем счет
                    if (player.score % 200 == 0 && player.score > 0)
                    {
                        map.AddLine(); // Добавляем новую строку блоков
                    }
                }
                player.dirY *= -1; // Обратное направление движения по оси Y
            }

            // Проверка столкновения с блоком на карте по оси X
            if (map.map[cellY, (player.ballX / CellSize) + player.dirX] != 0)
            {
                isColliding = true;

                // Обработка разрушения блока
                if (map.map[cellY, (player.ballX / CellSize) + player.dirX] > MinBrickValue && map.map[cellY + player.dirY, cellX] < MaxCellValue)
                {
                    map.map[cellY, (player.ballX / CellSize) + player.dirX] = 0; // Уничтожаем блок
                    map.map[cellY, (player.ballX / CellSize) + player.dirX - 1] = 0; // Уничтожаем блок слева
                    player.score += 50; // Увеличиваем счет
                    if (player.score % 200 == 0 && player.score > 0)
                    {
                        map.AddLine(); // Добавляем новую строку блоков
                    }
                }
                else if (map.map[cellY, (player.ballX / CellSize) + player.dirX] < MinBrickValue)
                {
                    map.map[cellY, (player.ballX / CellSize) + player.dirX] = 0; // Уничтожаем блок
                    map.map[cellY, (player.ballX / CellSize) + player.dirX + 1] = 0; // Уничтожаем блок справа
                    player.score += 50; // Увеличиваем счет
                    if (player.score % 200 == 0 && player.score > 0)
                    {
                        map.AddLine(); // Добавляем новую строку блоков
                    }
                }
                player.dirX *= -1; // Обратное направление движения по оси X
            }

            scoreLabel.Text = "Счет: " + player.score; // Обновление метки счета

            return isColliding;
        }
    }
}
