# Generated by Django 2.1.2 on 2018-10-11 14:02

from django.db import migrations, models
import django.db.models.deletion


class Migration(migrations.Migration):

    initial = True

    dependencies = [
    ]

    operations = [
        migrations.CreateModel(
            name='Now',
            fields=[
                ('id', models.AutoField(auto_created=True, primary_key=True, serialize=False, verbose_name='ID')),
            ],
        ),
        migrations.CreateModel(
            name='User',
            fields=[
                ('id', models.AutoField(auto_created=True, primary_key=True, serialize=False, verbose_name='ID')),
                ('userID', models.IntegerField(null=True)),
                ('name', models.CharField(default='foge', max_length=16, unique=True)),
                ('block', models.IntegerField(default=0, help_text='破壊したブロック数の合計')),
            ],
        ),
        migrations.AddField(
            model_name='now',
            name='userID',
            field=models.OneToOneField(default=1, on_delete=django.db.models.deletion.CASCADE, related_name='now', to='connect.User'),
        ),
    ]
